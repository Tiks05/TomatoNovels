using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TomatoNovels.Core.Exceptions;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.WriterInfo;
using TomatoNovels.Models;

namespace TomatoNovels.Services.Impl
{
    /// <summary>
    /// 作者详情页相关服务实现
    /// 对应原 Flask 的 writerinfo_service.py
    /// </summary>
    public class WriterInfoService : IWriterInfoService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Random _random = new Random();

        public WriterInfoService(
            AppDbContext db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 公共方法

        /// <summary>
        /// 获取作者头部信息
        /// </summary>
        public async Task<WriterDto> GetWriterHeaderAsync(int writerId)
        {
            // 只允许 role = author 的用户
            var user = await _db.Set<User>()
                .FirstOrDefaultAsync(u => u.Id == writerId && u.Role == "author");

            if (user == null)
            {
                // 对应原来查不到就会报错，这里用业务异常
                throw new ApiException("作者不存在或尚未成为作者", 40004);
            }

            // 总字数 = 所有书籍的 word_count 之和（和 Python 版保持一致）
            var totalWords = await _db.Set<Book>()
                .Where(b => b.UserId == writerId)
                .SumAsync(b => (int?)b.WordCount) ?? 0;

            // 粉丝数：目前是模拟值（30000 ~ 150000）
            var followerCount = _random.Next(30000, 150001);

            var host = GetHostUrl();
            var avatarUrl = string.IsNullOrEmpty(user.Avatar)
                ? string.Empty
                : $"{host}{user.Avatar}";

            var dto = new WriterDto
            {
                Nickname = user.Nickname ?? string.Empty,
                AvatarUrl = avatarUrl,
                Signature = user.Signature ?? string.Empty,
                // Python 版里 intro = signature，如果你有 Intro 字段可以换成 user.Intro
                Intro = user.Signature ?? string.Empty,
                BecomeAuthorAt = user.BecomeAuthorAt.HasValue
                    // 建议 ISO 字符串
                    ? user.BecomeAuthorAt.Value.ToString("O")
                    : string.Empty,
                TotalWords = totalWords,
                FollowerCount = followerCount
            };

            return dto;
        }

        /// <summary>
        /// 获取作者全部作品列表
        /// </summary>
        public async Task<List<WorkDto>> GetWriterWorksAsync(int writerId)
        {
            var host = GetHostUrl();

            // 找到该作者的所有书籍
            var books = await _db.Set<Book>()
                .Where(b => b.UserId == writerId)
                .ToListAsync();

            var result = new List<WorkDto>();

            foreach (var book in books)
            {
                // 最大章节号：max(chapter_num)，需要通过 Volume 关联 Book
                var maxChapterNum = await (
                    from c in _db.Set<Chapter>()
                    join v in _db.Set<Volume>() on c.VolumeId equals v.Id
                    where v.BookId == book.Id
                    select (int?)c.ChapterNum
                ).MaxAsync() ?? 0;

                string? maxChapterTitle = null;

                if (maxChapterNum > 0)
                {
                    // 查出最大章节对应的标题
                    var maxChapter = await (
                        from c in _db.Set<Chapter>()
                        join v in _db.Set<Volume>() on c.VolumeId equals v.Id
                        where v.BookId == book.Id && c.ChapterNum == maxChapterNum
                        select c
                    ).FirstOrDefaultAsync();

                    if (maxChapter != null)
                    {
                        maxChapterTitle = maxChapter.Title;
                    }
                }

                // 总字数：动态统计该书所有章节的 word_count
                var totalWordCount = await (
                    from c in _db.Set<Chapter>()
                    join v in _db.Set<Volume>() on c.VolumeId equals v.Id
                    where v.BookId == book.Id
                    select (int?)c.WordCount
                ).SumAsync() ?? 0;

                var coverUrl = string.IsNullOrEmpty(book.CoverUrl)
                    ? string.Empty
                    : $"{host}{book.CoverUrl}";

                var workDto = new WorkDto
                {
                    Title = book.Title ?? string.Empty,
                    CoverUrl = coverUrl,
                    Status = book.Status ?? string.Empty,
                    WordCount = totalWordCount,
                    Tags = book.Tags ?? string.Empty,
                    Intro = book.Intro ?? string.Empty,
                    UpdatedAt = book.UpdatedAt.HasValue
                        ? book.UpdatedAt.Value.ToString("O")
                        : string.Empty,
                    BookInfoPath = $"/bookinfo/{book.Id}",
                    MaxChapter = maxChapterNum == 0 ? null : maxChapterNum,
                    MaxChapterTitle = maxChapterTitle
                };

                result.Add(workDto);
            }

            return result;
        }

        #endregion

        #region 私有工具方法

        /// <summary>
        /// 获取当前请求的 HostUrl，例如：
        ///   https://localhost:7014
        /// 末尾不带斜杠
        /// </summary>
        private string GetHostUrl()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return string.Empty;
            }

            var request = httpContext.Request;
            var host = $"{request.Scheme}://{request.Host}".TrimEnd('/');
            return host;
        }

        #endregion
    }
}
