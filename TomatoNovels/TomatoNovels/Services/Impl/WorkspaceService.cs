using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomatoNovels.Core.Exceptions;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Workspace;
using TomatoNovels.Models;
using TomatoNovels.Utils;

namespace TomatoNovels.Services.Impl
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public WorkspaceService(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        private string BuildAbsoluteUrl(string? relative)
        {
            if (string.IsNullOrWhiteSpace(relative)) return "";
            var http = _http.HttpContext;
            if (http == null) return relative;

            var scheme = http.Request.Scheme;
            var host = http.Request.Host.Value;

            if (!relative.StartsWith("/"))
                relative = "/" + relative;

            return $"{scheme}://{host}{relative}";
        }

        public async Task<AuthorApplyResultDto> SaveAuthorApplicationAsync(
            int userId,
            string name,
            string intro,
            IFormFile? avatarFile,
            string fallbackAvatar
        )
        {
            string relativePath;
            string avatarUrl;

            // 1. 头像上传处理（优先使用用户新上传的文件）
            if (avatarFile != null)
            {
                // 对应 Python：save_uploaded_image(file=avatar_file, sub_folder='user/avatars')
                var (rel, url) = await ImageUtils.SaveUploadedImageAsync(
                    avatarFile,
                    "user/avatars",
                    _http    // IHttpContextAccessor
                );
                relativePath = rel;   // 存数据库
                avatarUrl = url;      // 返回前端
            }
            else
            {
                // 没传新头像，fallbackAvatar 可能是完整 URL，需要把 host 去掉还原为相对路径
                if (!string.IsNullOrWhiteSpace(fallbackAvatar))
                {
                    var httpContext = _http.HttpContext;
                    if (httpContext != null)
                    {
                        var hostBase = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}".TrimEnd('/');
                        // Python: fallback_avatar.replace(request.host_url.rstrip('/'), '')
                        relativePath = fallbackAvatar.Replace(hostBase, "");
                    }
                    else
                    {
                        relativePath = fallbackAvatar;
                    }
                }
                else
                {
                    relativePath = "";
                }

                // 用相对路径重新拼绝对 URL（和 Python 一样：host_url.rstrip('/') + relative）
                avatarUrl = string.IsNullOrWhiteSpace(relativePath)
                    ? ""
                    : BuildAbsoluteUrl(relativePath);
            }

            // 2. 更新数据库中的用户信息
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new ApiException("用户不存在", 40401);

            user.Nickname = name;
            user.Signature = intro;
            user.Avatar = relativePath;
            user.Role = "author";
            user.AuthorLevel = "签约作家";
            user.BecomeAuthorAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            // 3. 返回结构化数据（绝对路径给前端）
            return new AuthorApplyResultDto
            {
                Avatar = avatarUrl,
                Nickname = user.Nickname ?? "",
                BecomeAuthorAt = user.BecomeAuthorAt?.ToString("o"),
                Signature = user.Signature ?? ""
            };
        }

        public async Task<AuthorStatsDto> GetAuthorStatsAsync(int writerId)
        {
            var totalWords = await _db.Books
                .Where(b => b.UserId == writerId)
                .SumAsync(b => (int?)b.WordCount ?? 0);

            // Python: random.randint(30000, 150000)
            var random = new Random();
            var fans = random.Next(30000, 150000);

            return new AuthorStatsDto
            {
                FansCount = fans,
                TotalWords = totalWords
            };
        }

        public async Task<List<NoticeItemDto>> GetNoticeListAsync(int limit)
        {
            var http = _http.HttpContext;

            var list = await _db.News
                .Where(n => n.Type == "active" && n.IsNotice == true)
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .ToListAsync();

            return list.Select(n => new NoticeItemDto
            {
                NoticeUrl = !string.IsNullOrWhiteSpace(n.NoticeUrl)
                    ? BuildAbsoluteUrl(n.NoticeUrl)
                    : "",
                Title = n.Title,
                Time = $"{n.UpdatedAt:MM.dd} - 10.03",
                Path = $"/newsinfo/{n.Id}"
            }).ToList();
        }

        public async Task<List<NewsListItemDto>> GetNewsListAsync(int limit)
        {
            return await _db.News
                .Where(n => n.Type == "active")
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .Select(n => new NewsListItemDto
                {
                    Title = n.Title,
                    Path = $"/newsinfo/{n.Id}"
                })
                .ToListAsync();
        }

        public async Task<BookRankResponseDto> GetBookRankDataAsync(string readerType, string category)
        {
            var books = await _db.Books
                .Include(b => b.User)
                .Where(b => b.ReaderType == readerType)
                .Where(b => b.PlotType == category)
                .Where(b => b.Status == "连载中")
                .OrderByDescending(b => b.CreatedAt)
                .Take(4)
                .ToListAsync();

            var child = books.Select((b, idx) => new SortItemDto
            {
                Num = idx + 1,
                Title = b.Title,
                Path = $"/bookinfo/{b.Id}",
                Pic = BuildAbsoluteUrl(b.CoverUrl),
                Author = b.User?.Nickname ?? "",
                Desc = b.Intro ?? ""
            }).ToList();

            return new BookRankResponseDto
            {
                PlotType = category,
                Child = child
            };
        }

        public async Task SaveBookAsync(BookCreateFormDto form, IFormFile? coverFile)
        {
            string coverUrlRelative;

            // 1. 封面上传处理
            if (coverFile != null)
            {
                // Python：save_uploaded_image(file=cover_file, sub_folder='/covers')
                var (rel, _) = await ImageUtils.SaveUploadedImageAsync(
                    coverFile,
                    "covers",   // 注意这里不用加斜杠，工具里会自己拼成 static/uploads/covers/...
                    _http
                );
                coverUrlRelative = rel;
            }
            else
            {
                coverUrlRelative = "/uploads/covers/default_cover.png";
            }

            // 2. 构造主角字段（合并 hero1 + hero2）
            var hero = string.Join(" / ", new[] { form.Hero1?.Trim(), form.Hero2?.Trim() }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            // 3. 创建 Book 实例
            var book = new Book
            {
                UserId = form.Id,
                Title = form.Name.Trim(),
                ReaderType = form.ReaderType,
                Tags = form.Tag.Trim(),
                Intro = string.IsNullOrWhiteSpace(form.Introduction)
                    ? "新作品出炉，欢迎大家前往番茄小说阅读我的作品，希望大家能够喜欢，你们的关注是我写作的动力，我会努力讲好每个故事！"
                    : form.Introduction.Trim(),
                Hero = hero,
                CoverUrl = coverUrlRelative,
                Status = "连载中",
                WordCount = 0,
                WordCountRange = "30万以下",
                CreatedAt = DateTime.UtcNow
            };

            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }

        public async Task<List<BookListItemDto>> GetBookListByUserAsync(int userId)
        {
            var http = _http.HttpContext;

            var books = await _db.Books
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var result = new List<BookListItemDto>();

            foreach (var book in books)
            {
                // 所有 volume_id
                var volumeIds = await _db.Volumes
                    .Where(v => v.BookId == book.Id)
                    .Select(v => v.Id)
                    .ToListAsync();

                Chapter? recent = null;
                int chapterCount = 0;
                int totalWords = 0;

                if (volumeIds.Count > 0)
                {
                    // 最近章节
                    recent = await _db.Chapters
                        .Where(c => volumeIds.Contains(c.VolumeId))
                        .OrderByDescending(c => c.CreatedAt)
                        .FirstOrDefaultAsync();

                    // 章节数量
                    chapterCount = await _db.Chapters
                        .Where(c => volumeIds.Contains(c.VolumeId))
                        .CountAsync();

                    // 字数统计
                    totalWords = await _db.Chapters
                        .Where(c => volumeIds.Contains(c.VolumeId))
                        .SumAsync(c => (int?)c.WordCount ?? 0);
                }

                result.Add(new BookListItemDto
                {
                    Title = book.Title,
                    Pic = !string.IsNullOrWhiteSpace(book.CoverUrl)
                        ? BuildAbsoluteUrl(book.CoverUrl)
                        : "/src/assets/images/workspace/writer/default_cover.png",
                    Now = recent != null ? recent.UpdatedAt.ToString("yyyy-MM-dd HH:mm") : "",
                    Chapter = recent?.ChapterNum ?? 0,
                    Words = totalWords,
                    Status = book.Status ?? "连载中",
                    Path = $"/bookinfo/{book.Id}"
                });
            }

            return result;
        }

        public async Task<BookDetailDto> GetBookDetailAsync(int bookId)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId)
                ?? throw new ApiException("书籍不存在", 40401);

            return new BookDetailDto
            {
                Id = book.Id,
                Title = book.Title,
                CoverUrl = BuildAbsoluteUrl(book.CoverUrl),
                TargetReaders = book.ReaderType ?? "-",
                Tags = book.Tags ?? "-",
                MainRoles = book.Hero ?? "-",
                Intro = book.Intro ?? "",
                CreatedAt = book.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                Status = "正常",
                ContractStatus = book.SignStatus ?? "未签约",
                UpdateStatus = book.Status ?? "连载中"
            };
        }

        public async Task DeleteBookByIdAsync(int bookId)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId)
                ?? throw new ApiException("书籍不存在", 40401);

            // 分卷 ID
            var volumeIds = await _db.Volumes
                .Where(v => v.BookId == bookId)
                .Select(v => v.Id)
                .ToListAsync();

            if (volumeIds.Any())
            {
                // 删除章节
                var chapters = _db.Chapters.Where(c => volumeIds.Contains(c.VolumeId));
                _db.Chapters.RemoveRange(chapters);
            }

            // 删分卷
            var volumes = _db.Volumes.Where(v => v.BookId == bookId);
            _db.Volumes.RemoveRange(volumes);

            // 删书籍
            _db.Books.Remove(book);

            await _db.SaveChangesAsync();
        }

        public async Task UpdateBookInfoAsync(BookUpdateFormDto form, IFormFile? coverFile)
        {
            // 1. 查找原书籍
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == form.BookId)
                ?? throw new ApiException("书籍不存在", 40401);

            // 2. 封面上传（如有则替换）
            if (coverFile != null)
            {
                var (rel, _) = await ImageUtils.SaveUploadedImageAsync(
                    coverFile,
                    "covers",
                    _http
                );
                book.CoverUrl = rel;
            }

            // 3. 主角名合并（你在更新的时候 schema 里是 hero 一整段）
            book.Hero = form.Hero.Trim();

            // 4. 其他字段更新
            book.Title = form.Name.Trim();
            book.ReaderType = form.ReaderType;
            book.Tags = form.Tag.Trim();
            book.Intro = form.Introduction.Trim();

            // 5. 提交修改
            await _db.SaveChangesAsync();
        }

        public async Task<ChapterInfoDto> GetLastChapterInfoAsync(int bookId)
        {
            var volumeIdsQuery = _db.Volumes
                .Where(v => v.BookId == bookId)
                .Select(v => v.Id);

            var lastChapter = await _db.Chapters
                .Where(c => volumeIdsQuery.Contains(c.VolumeId))
                .Include(c => c.Volume)
                .OrderByDescending(c => c.Volume.Sort)
                .ThenByDescending(c => c.ChapterNum)
                .FirstOrDefaultAsync();

            if (lastChapter == null)
                return new ChapterInfoDto();

            return new ChapterInfoDto
            {
                VolumeIndex = lastChapter.Volume.Sort,
                VolumeTitle = lastChapter.Volume.Title,
                ChapterIndex = lastChapter.ChapterNum,
                ChapterTitle = lastChapter.Title
            };
        }

        public async Task CreateChapterAsync(ChapterCreateDto dto)
        {
            Volume volume;

            if (dto.VolumeId != null)
            {
                volume = await _db.Volumes.FirstOrDefaultAsync(v => v.Id == dto.VolumeId)
                    ?? throw new ApiException("指定的分卷不存在", 40004);
            }
            else
            {
                // 找最后一卷
                volume = await _db.Volumes
                    .Where(v => v.BookId == dto.BookId)
                    .OrderByDescending(v => v.Sort)
                    .FirstOrDefaultAsync();

                // 没有分卷 → 创建第一卷
                if (volume == null)
                {
                    volume = new Volume
                    {
                        BookId = dto.BookId,
                        Title = "第一卷",
                        Sort = 1,
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.Volumes.Add(volume);
                    await _db.SaveChangesAsync();
                }
            }

            // 最后章节号
            var last = await _db.Chapters
                .Where(c => c.VolumeId == volume.Id)
                .OrderByDescending(c => c.ChapterNum)
                .FirstOrDefaultAsync();

            var nextNum = (last?.ChapterNum ?? 0) + 1;

            var chapter = new Chapter
            {
                VolumeId = volume.Id,
                ChapterNum = nextNum,
                Title = dto.Title,
                Content = dto.Content,
                WordCount = dto.WordCount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "published"
            };

            _db.Chapters.Add(chapter);
            await _db.SaveChangesAsync();
        }

        private readonly Dictionary<string, string> _statusMap = new()
        {
            ["published"] = "已发布",
            ["reviewing"] = "审核中",
            ["rejected"] = "审核不通过",
            ["pending"] = "待发布"
        };

        public async Task<ChapterListResponseDto> GetChapterListByBookIdAsync(
            int bookId,
            string title,
            string volumeId,
            string status
        )
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId)
                ?? throw new ApiException("书籍不存在", 40401);

            var volumes = await _db.Volumes
                .Where(v => v.BookId == bookId)
                .OrderBy(v => v.Sort)
                .ToListAsync();

            var volumeItems = volumes.Select(v => new VolumeItemDto
            {
                Id = v.Id,
                BookId = v.BookId,
                Title = v.Title,
                Sort = v.Sort,
                CreatedAt = v.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            var q = _db.Chapters.Include(c => c.Volume)
                .Where(c => c.Volume.BookId == bookId);

            if (!string.IsNullOrWhiteSpace(title))
                q = q.Where(c => EF.Functions.Like(c.Title, $"%{title}%"));

            if (!string.IsNullOrWhiteSpace(volumeId))
                q = q.Where(c => c.VolumeId == int.Parse(volumeId));

            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(c => c.Status == status);

            var chapters = await q.OrderBy(c => c.ChapterNum).ToListAsync();

            var chapterItems = chapters.Select(c => new ChapterItemDto
            {
                Id = c.Id,
                VolumeId = c.VolumeId,
                ChapterNum = c.ChapterNum,
                Title = c.Title,
                WordCount = c.WordCount ?? (c.Content?.Length ?? 0),
                UpdatedAt = c.UpdatedAt.ToString("yyyy-MM-dd HH:mm"),
                Status = c.Status,
                StatusText = _statusMap.ContainsKey(c.Status) ? _statusMap[c.Status] : "未知状态",
                TypoCount = 0
            }).ToList();

            return new ChapterListResponseDto
            {
                Title = book.Title,
                Volumes = volumeItems,
                List = chapterItems
            };
        }

        public async Task<bool> DeleteChapterByIdAsync(int chapterId)
        {
            var chapter = await _db.Chapters.FirstOrDefaultAsync(c => c.Id == chapterId)
                ?? throw new ApiException("章节不存在", 40404);

            _db.Chapters.Remove(chapter);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateChapterAsync(ChapterUpdateDto dto)
        {
            var chapter = await _db.Chapters.FirstOrDefaultAsync(c => c.Id == dto.ChapterId)
                ?? throw new ApiException("章节不存在", 40404);

            var volume = await _db.Volumes.FirstOrDefaultAsync(v => v.Id == chapter.VolumeId);
            if (volume == null || volume.BookId != dto.BookId)
                throw new ApiException("章节不属于该书籍", 40303);

            chapter.ChapterNum = dto.ChapterNum;
            chapter.Title = dto.Title;
            chapter.Content = dto.Content;
            chapter.WordCount = dto.WordCount;
            chapter.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task<ChapterDetailDto> GetChapterDetailByIdAsync(int bookId, int chapterId)
        {
            var chapter = await _db.Chapters
                .Include(c => c.Volume)
                .FirstOrDefaultAsync(c => c.Id == chapterId)
                ?? throw new ApiException("章节不存在", 40404);

            if (chapter.Volume.BookId != bookId)
                throw new ApiException("章节不属于该书籍", 40303);

            return new ChapterDetailDto
            {
                VolumeIndex = chapter.Volume.Sort,
                VolumeTitle = chapter.Volume.Title,
                ChapterNum = chapter.ChapterNum,
                Title = chapter.Title,
                Content = chapter.Content
            };
        }

        public async Task<bool> DeleteVolumeWithChaptersAsync(int bookId, int volumeId)
        {
            var volume = await _db.Volumes
                .FirstOrDefaultAsync(v => v.Id == volumeId && v.BookId == bookId);

            if (volume == null) return false;

            var chapters = _db.Chapters.Where(c => c.VolumeId == volumeId);
            _db.Chapters.RemoveRange(chapters);

            _db.Volumes.Remove(volume);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateVolumeTitleAsync(int volumeId, int bookId, string newTitle)
        {
            var volume = await _db.Volumes.FirstOrDefaultAsync(v => v.Id == volumeId && v.BookId == bookId);
            if (volume == null) return false;

            volume.Title = newTitle;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateVolumeAsync(int bookId, string title, int sort)
        {
            var volume = new Volume
            {
                BookId = bookId,
                Title = title,
                Sort = sort,
                CreatedAt = DateTime.UtcNow
            };

            _db.Volumes.Add(volume);
            await _db.SaveChangesAsync();

            return volume.Id;
        }

        public async Task<LastChapterResponseDto?> GetLastChapterByBookIdAsync(int bookId)
        {
            var lastVolume = await _db.Volumes
                .Where(v => v.BookId == bookId)
                .OrderByDescending(v => v.Sort)
                .FirstOrDefaultAsync();

            if (lastVolume == null) return null;

            var lastChapter = await _db.Chapters
                .Where(c => c.VolumeId == lastVolume.Id)
                .OrderByDescending(c => c.ChapterNum)
                .FirstOrDefaultAsync();

            return new LastChapterResponseDto
            {
                LastVolumeId = lastVolume.Sort,
                LastVolumeTitle = lastVolume.Title,
                ChapterIndex = lastChapter?.ChapterNum ?? 0,
                ChapterTitle = lastChapter?.Title ?? "",
                UpdatedAt = lastChapter?.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? ""
            };
        }

        public async Task<LastChapterResponseDto?> GetLastChapterByVolumeIdAsync(int bookId, int volumeId)
        {
            var currentVolume = await _db.Volumes
                .FirstOrDefaultAsync(v => v.Id == volumeId && v.BookId == bookId);

            if (currentVolume == null) return null;

            var lastVolume = await _db.Volumes
                .Where(v => v.BookId == bookId)
                .OrderByDescending(v => v.Sort)
                .FirstOrDefaultAsync();

            var lastChapter = await _db.Chapters
                .Where(c => c.VolumeId == lastVolume.Id)
                .OrderByDescending(c => c.ChapterNum)
                .FirstOrDefaultAsync();

            return new LastChapterResponseDto
            {
                VolumeTitle = currentVolume.Title,
                CurrentVolumeId = currentVolume.Sort,
                LastVolumeId = lastVolume.Sort,
                LastVolumeTitle = lastVolume.Title,
                ChapterIndex = lastChapter?.ChapterNum ?? 0,
                ChapterTitle = lastChapter?.Title ?? "",
                UpdatedAt = lastChapter?.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? ""
            };
        }

        public async Task<LatestChapterResponseDto?> GetLatestChapterByBookIdAsync(int bookId)
        {
            var result = await _db.Chapters
                .Include(c => c.Volume)
                .Where(c => c.Volume.BookId == bookId)
                .OrderByDescending(c => c.UpdatedAt)
                .FirstOrDefaultAsync();

            if (result == null) return null;

            return new LatestChapterResponseDto
            {
                LatestVolumeSort = result.Volume.Sort,
                LatestChapterNum = result.ChapterNum,
                LatestChapterTitle = result.Title,
                LatestChapterUpdatedAt = result.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}