using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TomatoNovels.Shared.DTOs.BookInfo;
using TomatoNovels.Core.Exceptions;
using TomatoNovels.Data;
using TomatoNovels.Models;

namespace TomatoNovels.Services
{
    public class BookInfoService : IBookInfoService
    {
        private readonly AppDbContext _db;                // TODO: 换成你的实际 DbContext 类型，比如 TomatoDbContext
        private readonly IHttpContextAccessor _http;   // 用来拼接 hostUrl

        public BookInfoService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _http = httpContextAccessor;
        }

        #region 公共方法

        public async Task<BookHeaderDto> GetBookHeaderAsync(int bookId)
        {
            var book = await _db.Set<Book>()
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                // Python 里是直接返回 {}，这里用业务异常更安全
                throw new ApiException("书籍不存在", 40401);
            }

            // 最新章节（按更新时间倒序）
            var latestChapter = await _db.Set<Chapter>()
                .Include(c => c.Volume)
                .Where(c => c.Volume.BookId == bookId)
                .OrderByDescending(c => c.UpdatedAt)
                .FirstOrDefaultAsync();

            // 总字数
            var totalWordCount = await _db.Set<Chapter>()
                .Include(c => c.Volume)
                .Where(c => c.Volume.BookId == bookId)
                .SumAsync(c => (int?)c.WordCount) ?? 0;

            var author = book.User;

            // 拼接 hostUrl（对应 Flask 的 request.host_url.rstrip('/')）
            var baseUrl = GetBaseUrl();

            var dto = new BookHeaderDto
            {
                Book = new BookHeaderBookDto
                {
                    Id = book.Id,
                    Title = book.Title ?? "",
                    CoverUrl = !string.IsNullOrWhiteSpace(book.CoverUrl)
                        ? $"{baseUrl}{book.CoverUrl}"
                        : "",
                    Status = book.Status ?? "",
                    WordCount = totalWordCount,
                    Tags = book.Tags ?? "",
                    UpdatedAt = book.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    LatestChapter = latestChapter?.ChapterNum ?? 0,
                    LatestChapterTitle = latestChapter?.Title ?? ""
                },
                Author = new BookHeaderAuthorDto
                {
                    Nickname = author?.Nickname ?? "",
                    CoverUrl = (author != null && !string.IsNullOrWhiteSpace(author.Avatar))
                        ? $"{baseUrl}{author.Avatar}"
                        : "",
                    Signature = author?.Signature ?? "",
                    Path = author != null ? $"/writerinfo/{author.Id}" : ""
                }
            };

            return dto;
        }

        public async Task<BookContentDto> GetBookContentAsync(int bookId)
        {
            // 取所有 Volume（按 sort 升序），并包含 Chapters
            var volumes = await _db.Set<Volume>()
                .Include(v => v.Chapters)
                .Where(v => v.BookId == bookId)
                .OrderBy(v => v.Sort)
                .ToListAsync();

            var result = new BookContentDto();

            int index = 1; // 用来转中文“第一卷、第二卷……”
            foreach (var vol in volumes)
            {
                // 章节按 chapter_num 排序
                var chapters = vol.Chapters
                    .OrderBy(c => c.ChapterNum)
                    .ToList();

                var chapterList = chapters
                    .Select(ch => new BookChapterItemDto
                    {
                        Title = ch.Title ?? "",
                        // 对应 Python：f'/read/{book_id}/{vol.sort}/{chap.chapter_num}'
                        Path = $"/read/{bookId}/{vol.Sort}/{ch.ChapterNum}"
                    })
                    .ToList();

                var chineseIdx = NumToChinese(index);

                result.Volumes.Add(new BookVolumeDto
                {
                    Title = $"第{chineseIdx}卷：{vol.Title}",
                    ChapterCount = chapterList.Count,
                    Chapters = chapterList
                });

                index++;
            }

            // intro
            var book = await _db.Set<Book>().FirstOrDefaultAsync(b => b.Id == bookId);
            result.Intro = book?.Intro ?? "";

            return result;
        }

        public async Task<ChapterReadResponseDto> GetChapterContentAsync(int bookId, int volumeSort, int chapterNum)
        {
            // 1. 通过 bookId + volume.sort 找 Volume
            var volume = await _db.Set<Volume>()
                .Include(v => v.Book)
                .FirstOrDefaultAsync(v => v.BookId == bookId && v.Sort == volumeSort);

            if (volume == null)
            {
                throw new ApiException("该分卷不存在或不属于当前书籍", 40010);
            }

            // 2. 通过 volume.id + chapterNum 找 Chapter
            var chapter = await _db.Set<Chapter>()
                .FirstOrDefaultAsync(c => c.VolumeId == volume.Id && c.ChapterNum == chapterNum);

            if (chapter == null)
            {
                throw new ApiException("章节不存在或不属于该分卷", 40011);
            }

            // 3. 书名
            var bookTitle = volume.Book?.Title ?? "";

            // 4. 上一章
            var prevChapter = await _db.Set<Chapter>()
                .Where(c => c.VolumeId == volume.Id && c.ChapterNum < chapter.ChapterNum)
                .OrderByDescending(c => c.ChapterNum)
                .FirstOrDefaultAsync();

            // 5. 下一章
            var nextChapter = await _db.Set<Chapter>()
                .Where(c => c.VolumeId == volume.Id && c.ChapterNum > chapter.ChapterNum)
                .OrderBy(c => c.ChapterNum)
                .FirstOrDefaultAsync();

            // 6. 组织响应
            var dto = new ChapterReadResponseDto
            {
                BookTitle = bookTitle,
                ChapterTitle = chapter.Title ?? "",
                WordCount = chapter.WordCount ?? (chapter.Content?.Length ?? 0),
                UpdatedAt = chapter.UpdatedAt.ToString("yyyy-MM-dd") ?? "",
                Content = chapter.Content ?? "",
                ChapterIndex = chapter.ChapterNum,
                PrevChapterId = prevChapter?.ChapterNum,
                NextChapterId = nextChapter?.ChapterNum
            };

            return dto;
        }

        #endregion

        #region 私有工具方法（对应 Python 里的小函数）

        /// <summary>
        /// 对应 Flask 的 request.host_url.rstrip('/')，返回如 https://localhost:7014
        /// </summary>
        private string GetBaseUrl()
        {
            var httpContext = _http.HttpContext;
            if (httpContext == null)
            {
                return "";
            }

            var request = httpContext.Request;
            var root = $"{request.Scheme}://{request.Host}";
            return root.TrimEnd('/');
        }

        /// <summary>
        /// 对应 Python 中的 num_to_chinese（只实现到两位数够用了）
        /// </summary>
        private string NumToChinese(int num)
        {
            var digits = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

            if (num <= 10)
            {
                return num == 10 ? "十" : digits[num];
            }
            else if (num < 20)
            {
                return "十" + digits[num % 10];
            }
            else
            {
                var tens = num / 10;
                var ones = num % 10;
                return digits[tens] + "十" + (ones > 0 ? digits[ones] : "");
            }
        }

        #endregion
    }
}
