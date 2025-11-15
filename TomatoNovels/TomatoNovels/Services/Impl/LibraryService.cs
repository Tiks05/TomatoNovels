using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Library;
using TomatoNovels.Models;

namespace TomatoNovels.Services.Impl
{
    public class LibraryService : ILibraryService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public LibraryService(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        public async Task<BookListResultDto> GetFilteredBooksAsync(BookListQueryDto query)
        {
            // 基础查询 + 关联作者
            IQueryable<Book> q = _db.Books
                .Include(b => b.User)
                .AsQueryable();

            // 1) reader_type
            if (!string.IsNullOrWhiteSpace(query.ReaderType))
            {
                q = q.Where(b => b.ReaderType == query.ReaderType);
            }

            // 2) 分类 group + type
            if (!string.IsNullOrWhiteSpace(query.CategoryGroup) &&
                !string.IsNullOrWhiteSpace(query.CategoryType))
            {
                switch (query.CategoryGroup)
                {
                    case "theme_type":
                        q = q.Where(b => b.ThemeType == query.CategoryType);
                        break;
                    case "role_type":
                        q = q.Where(b => b.RoleType == query.CategoryType);
                        break;
                    case "plot_type":
                        q = q.Where(b => b.PlotType == query.CategoryType);
                        break;
                }
            }

            // 3) 状态
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                q = q.Where(b => b.Status == query.Status);
            }

            // 4) 字数区间
            if (!string.IsNullOrWhiteSpace(query.WordCountRange))
            {
                switch (query.WordCountRange)
                {
                    case "30万以下":
                        q = q.Where(b => b.WordCount < 300_000);
                        break;
                    case "30-50万":
                        q = q.Where(b => b.WordCount >= 300_000 && b.WordCount <= 500_000);
                        break;
                    case "50-100万":
                        q = q.Where(b => b.WordCount >= 500_000 && b.WordCount <= 1_000_000);
                        break;
                    case "100-200万":
                        q = q.Where(b => b.WordCount >= 1_000_000 && b.WordCount <= 2_000_000);
                        break;
                    case "200万以上":
                        q = q.Where(b => b.WordCount >= 2_000_000);
                        break;
                }
            }

            // 5) 排序
            if (!string.IsNullOrWhiteSpace(query.Sort))
            {
                switch (query.Sort)
                {
                    case "hot":
                        q = q.OrderByDescending(b => b.FavoriteCount);
                        break;
                    case "new":
                        q = q.OrderByDescending(b => b.UpdatedAt);
                        break;
                    case "words":
                        q = q.OrderByDescending(b => b.WordCount);
                        break;
                }
            }
            else
            {
                // 默认按更新时间倒序（与番茄行为一致）
                q = q.OrderByDescending(b => b.UpdatedAt);
            }

            // 总数
            var total = await q.CountAsync();

            // 分页 (Page 从 1 开始)
            var skip = (query.Page - 1) * query.PageSize;

            var books = await q
                .Skip(skip)
                .Take(query.PageSize)
                .ToListAsync();

            // 拼接返回
            var records = books.Select(b => ToBookOutDto(b)).ToList();

            return new BookListResultDto
            {
                Total = total,
                Records = records
            };
        }

        /// <summary>
        /// 把 Book 实体转换为 BookOutDto（对应 Python 的 serialize_book）
        /// </summary>
        private BookOutDto ToBookOutDto(Book book)
        {
            var updatedAt = book.UpdatedAt ?? DateTime.UtcNow;
            var now = DateTime.UtcNow;
            var delta = now - updatedAt;

            string timeDisplay;

            if (delta.TotalDays >= 1)
            {
                timeDisplay = updatedAt.ToString("yyyy-MM-dd HH:mm");
            }
            else if (delta.TotalHours >= 1)
            {
                timeDisplay = $"{(int)delta.TotalHours}小时前";
            }
            else if (delta.TotalMinutes >= 1)
            {
                timeDisplay = $"{(int)delta.TotalMinutes}分钟前";
            }
            else
            {
                timeDisplay = $"{(int)delta.TotalSeconds}秒前";
            }

            // 封面 URL 拼接 host_url
            var host = _http.HttpContext?.Request.Host.Value ?? "";
            var scheme = _http.HttpContext?.Request.Scheme ?? "http";

            string coverUrl = "";
            if (!string.IsNullOrWhiteSpace(book.CoverUrl))
            {
                // book.cover_url 存的是相对路径
                coverUrl = $"{scheme}://{host}{book.CoverUrl}";
            }

            return new BookOutDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.User?.Nickname ?? "",
                Status = book.Status ?? "",
                WordCount = book.WordCount ?? 0,
                Intro = book.Intro ?? "",
                CoverUrl = coverUrl,
                UpdatedAt = timeDisplay,
                Path = $"/bookinfo/{book.Id}"
            };
        }
    }
}
