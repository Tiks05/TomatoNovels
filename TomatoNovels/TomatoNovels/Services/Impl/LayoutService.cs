using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TomatoNovels.Controllers.Exceptions;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Layout;
using TomatoNovels.Models;

namespace TomatoNovels.Services.Impl
{
    /// <summary>
    /// Layout 相关业务：用户资料更新 + 搜索书籍
    /// 对应 Python 的 layout_service.py
    /// </summary>
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public LayoutService(
            AppDbContext db,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment env)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        #region 工具方法

        /// <summary>
        /// 获取 host 前缀，例如 https://localhost:7014
        /// </summary>
        private string GetHostPrefix()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx == null) return string.Empty;

            var req = ctx.Request;
            var hostUrl = $"{req.Scheme}://{req.Host.Value}";
            return hostUrl.TrimEnd('/');
        }

        /// <summary>
        /// 保存上传的头像，返回 (相对路径, 绝对 URL)
        /// 对应 Python: save_uploaded_image(file=..., sub_folder='user/avatars')
        ///   - 相对路径类似：/uploads/user/avatars/xxxxx.png
        ///   - 绝对 URL 类似：https://host/uploads/user/avatars/xxxxx.png
        /// </summary>
        private async Task<(string RelativePath, string AvatarUrl)> SaveUploadedImageAsync(
            IFormFile file,
            string subFolder)
        {
            if (file == null || file.Length == 0)
            {
                return (string.Empty, string.Empty);
            }

            // 物理路径：wwwroot/uploads/user/avatars/
            var webRoot = _env.WebRootPath;
            var folder = Path.Combine(webRoot, "uploads", subFolder.Trim('/').Replace('/', Path.DirectorySeparatorChar));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(folder, fileName);

            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            // 相对路径（存数据库）：/uploads/user/avatars/xxx.png
            var relativePath = $"/uploads/{subFolder.Trim('/')}/{fileName}";

            // 绝对 URL（给前端）：hostPrefix + 相对路径
            var avatarUrl = $"{GetHostPrefix()}{relativePath}";

            return (relativePath, avatarUrl);
        }

        #endregion

        /// <inheritdoc />
        public async Task<UserProfileUpdateResultDto> UpdateUserProfileAsync(
            int userId,
            string name,
            string introduction,
            IFormFile? avatarFile,
            string fallbackAvatar)
        {
            // ====== 1. 头像上传处理 ======
            string relativePath;
            string avatarUrl;

            if (avatarFile != null && avatarFile.Length > 0)
            {
                // 新头像：保存到 /uploads/user/avatars/
                (relativePath, avatarUrl) = await SaveUploadedImageAsync(avatarFile, "user/avatars");
            }
            else
            {
                // 没传新头像：从 fallback_avatar 中剥离 host，得到相对路径
                var hostPrefix = GetHostPrefix();

                if (string.IsNullOrWhiteSpace(fallbackAvatar))
                {
                    relativePath = string.Empty;
                    avatarUrl = string.Empty;
                }
                else
                {
                    relativePath = fallbackAvatar.Replace(hostPrefix, "");
                    avatarUrl = fallbackAvatar;
                }
            }

            // ====== 2. 更新数据库 ======
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ApiException("用户不存在", 40401);
            }

            user.Nickname = name;
            user.Signature = introduction;
            user.Avatar = relativePath;

            await _db.SaveChangesAsync();

            // ====== 3. 返回结构化数据 ======
            return new UserProfileUpdateResultDto
            {
                Avatar = avatarUrl,
                Nickname = user.Nickname ?? string.Empty,
                Signature = user.Signature ?? string.Empty
            };
        }

        /// <inheritdoc />
        public async Task<SearchBookResponseDto> SearchBooksAsync(SearchBookRequestDto data)
        {
            // 小写一下变量，方便对应 Python 的 data.xxx
            var keyword = data.Keyword;
            var type = data.Type ?? 0;
            var timeindex = data.TimeIndex ?? 0;
            var numindex = data.NumIndex ?? 0;
            var stateindex = data.StateIndex ?? 0;
            var page = data.Page;
            var pageSize = data.PageSize;

            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            // db.session.query(Book, User.nickname).join(User, Book.user_id == User.id)
            var query = from b in _db.Books
                        join u in _db.Users on b.UserId equals u.Id
                        select new { Book = b, Nickname = u.Nickname };

            // === 模糊搜索 ===
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query = query.Where(x =>
                    EF.Functions.Like(x.Book.Title, like) ||
                    EF.Functions.Like(x.Nickname, like));
            }

            // === 连载状态筛选 ===
            if (stateindex == 1)
            {
                query = query.Where(x => x.Book.Status == "已完结");
            }
            else if (stateindex == 2)
            {
                query = query.Where(x => x.Book.Status == "连载中");
            }

            // === 字数筛选 ===
            if (numindex == 1)
            {
                query = query.Where(x => x.Book.WordCount < 300000);
            }
            else if (numindex == 2)
            {
                query = query.Where(x => x.Book.WordCount >= 300000 && x.Book.WordCount <= 500000);
            }
            else if (numindex == 3)
            {
                query = query.Where(x => x.Book.WordCount >= 500000 && x.Book.WordCount <= 1000000);
            }
            else if (numindex == 4)
            {
                query = query.Where(x => x.Book.WordCount > 1000000);
            }

            // === 更新时间筛选 ===
            var now = DateTime.Now;

            if (timeindex == 1)
            {
                var start = now - TimeSpan.FromMinutes(30);
                query = query.Where(x => x.Book.UpdatedAt >= start);
            }
            else if (timeindex == 2)
            {
                var today = new DateTime(now.Year, now.Month, now.Day);
                query = query.Where(x => x.Book.UpdatedAt >= today);
            }
            else if (timeindex == 3)
            {
                var monday = new DateTime(now.Year, now.Month, now.Day)
                             - TimeSpan.FromDays((int)now.DayOfWeek == 0 ? 6 : (int)now.DayOfWeek - 1);
                query = query.Where(x => x.Book.UpdatedAt >= monday);
            }
            else if (timeindex == 4)
            {
                var firstDay = new DateTime(now.Year, now.Month, 1);
                query = query.Where(x => x.Book.UpdatedAt >= firstDay);
            }
            else if (timeindex == 5)
            {
                var jan1 = new DateTime(now.Year, 1, 1);
                query = query.Where(x => x.Book.UpdatedAt >= jan1);
            }

            // === 排序 ===
            if (type == 1)
            {
                // 最热：按收藏数
                query = query.OrderByDescending(x => x.Book.FavoriteCount);
            }
            else if (type == 2)
            {
                // 最新：按更新时间
                query = query.OrderByDescending(x => x.Book.UpdatedAt);
            }
            else
            {
                // 默认：按 id 倒序
                query = query.OrderByDescending(x => x.Book.Id);
            }

            // === 总数 & 分页 ===
            var total = await query.CountAsync();

            var booksPage = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var bookIds = booksPage.Select(x => x.Book.Id).ToList();

            // === 第一章路径映射 ===
            var firstChapters = await (
                from v in _db.Volumes
                join c in _db.Chapters on v.Id equals c.VolumeId
                where bookIds.Contains(v.BookId) && c.Status == "published"
                orderby v.BookId, v.Sort, c.ChapterNum
                select new
                {
                    v.BookId,
                    VolumeSort = v.Sort,
                    c.ChapterNum
                }
            ).ToListAsync();

            var pathMap = new Dictionary<int, string>();
            foreach (var row in firstChapters)
            {
                if (!pathMap.ContainsKey(row.BookId))
                {
                    pathMap[row.BookId] = $"/read/{row.BookId}/{row.VolumeSort}/{row.ChapterNum}";
                }
            }

            // === 最新章节标题 + 路径 ===
            // latest_time_subq
            var latestTimeQuery = (
                from v in _db.Volumes
                join c in _db.Chapters on v.Id equals c.VolumeId
                where bookIds.Contains(v.BookId) && c.Status == "published"
                group c by v.BookId
                into g
                select new
                {
                    BookId = g.Key,
                    LatestTime = g.Max(x => x.CreatedAt)
                }
            );

            var latestChapters = await (
                from v in _db.Volumes
                join c in _db.Chapters on v.Id equals c.VolumeId
                join lt in latestTimeQuery
                    on new { v.BookId, CreatedAt = c.CreatedAt }
                    equals new { BookId = lt.BookId, CreatedAt = lt.LatestTime }
                select new
                {
                    v.BookId,
                    c.Title,
                    VolumeSort = v.Sort,
                    c.ChapterNum
                }
            ).ToListAsync();

            var updateMap = new Dictionary<int, (string Title, string Path)>();
            foreach (var row in latestChapters)
            {
                var path = $"/read/{row.BookId}/{row.VolumeSort}/{row.ChapterNum}";
                updateMap[row.BookId] = (row.Title, path);
            }

            // === 构造响应数据 ===
            var hostPrefix = GetHostPrefix();
            var records = new List<SearchBookItemDto>();

            foreach (var item in booksPage)
            {
                var book = item.Book;
                var nickname = item.Nickname;

                updateMap.TryGetValue(book.Id, out var updateInfo);

                var coverRelative = !string.IsNullOrWhiteSpace(book.CoverUrl)
                    ? book.CoverUrl
                    : "/uploads/covers/default.png";

                var picUrl = $"{hostPrefix}{coverRelative}";

                var updatedAtStr = book.UpdatedAt.HasValue
                    ? book.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm")
                    : "";

                var firstReadPath = pathMap.TryGetValue(book.Id, out var firstPath)
                    ? firstPath
                    : $"/read/{book.Id}/1/1";

                var updateTitle = string.IsNullOrWhiteSpace(updateInfo.Title)
                    ? "暂无更新"
                    : updateInfo.Title;

                var updatePath = string.IsNullOrWhiteSpace(updateInfo.Path)
                    ? $"/read/{book.Id}/1/1"
                    : updateInfo.Path;

                records.Add(new SearchBookItemDto
                {
                    Title = book.Title,
                    Author = nickname,
                    Status = book.Status ?? "",
                    WordCount = book.WordCount ?? 0,
                    Intro = book.Intro ?? "",
                    UpdatedAt = updatedAtStr,
                    Pic = picUrl,
                    People = book.FavoriteCount,
                    Update = updateTitle,
                    Path = $"/bookinfo/{book.Id}",
                    ReadPath = firstReadPath,
                    UpdatePath = updatePath
                });
            }

            return new SearchBookResponseDto
            {
                Total = total,
                Records = records
            };
        }
    }
}
