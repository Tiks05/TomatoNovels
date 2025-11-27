using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Home;
using TomatoNovels.Models;
using TomatoNovels.Shared.DTOs.Home.Response;

namespace TomatoNovels.Services.Impl
{
    /// <summary>
    /// 首页相关业务逻辑，对应 Python 的 
    /// _service.py
    /// </summary>
    public class HomeService : IHomeService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Random _random = new();

        public HomeService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 工具方法

        private string GetHostPrefix()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx == null) return string.Empty;

            var req = ctx.Request;
            var hostUrl = $"{req.Scheme}://{req.Host.Value}";
            return hostUrl.TrimEnd('/');
        }

        private string BuildUrl(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            return $"{GetHostPrefix()}{relativePath}";
        }

        #endregion

        /// <inheritdoc />
        public async Task<List<TopBookResponseDto>> GetTopBooksAsync()
        {
            // 对应 get_top_books_service:
            // Book.query.order_by(Book.favorite_count.desc()).limit(30).all()
            var books = await _db.Books
                .OrderByDescending(b => b.FavoriteCount)
                .Take(30)
                .ToListAsync();

            var result = new List<TopBookResponseDto>();

            for (int i = 0; i < books.Count; i++)
            {
                var book = books[i];

                // 取 tags 的第一个标签作为 desc
                var descValue = "未知分类";
                if (!string.IsNullOrWhiteSpace(book.Tags))
                {
                    descValue = book.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .FirstOrDefault() ?? "未知分类";
                }

                var coverUrl = BuildUrl(book.CoverUrl);

                var item = new TopBookResponseDto
                {
                    Num = (i + 1).ToString("D2"),
                    Title = book.Title,
                    Desc = descValue,
                    Path = $"/bookinfo/{book.Id}",
                    Pic = coverUrl
                };

                result.Add(item);
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<List<NewsResponseDto>> GetNewsListAsync(int limit)
        {
            // News.query.order_by(News.created_at.desc()).limit(limit)
            var newsList = await _db.News
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .ToListAsync();

            return newsList.Select(n => new NewsResponseDto
            {
                Title = n.Title,
                Path = $"/newsinfo/{n.Id}"
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<List<WriterResponseDto>> GetWriterListAsync()
        {
            // palace_writers = User.query.filter(User.author_level == '殿堂作家').all()
            // golden_writers = User.query.filter(User.author_level == '金番作家').all()
            var palace = await _db.Users
                .Where(u => u.AuthorLevel == "殿堂作家")
                .ToListAsync();

            var golden = await _db.Users
                .Where(u => u.AuthorLevel == "金番作家")
                .ToListAsync();

            var writers = palace.Concat(golden).ToList();

            var hostPrefix = GetHostPrefix();

            return writers.Select(w => new WriterResponseDto
            {
                Title = w.Nickname ?? "",
                Desc = string.IsNullOrWhiteSpace(w.Masterpiece)
                        ? ""
                        : $"代表作{w.Masterpiece}",
                Type = w.AuthorLevel ?? "",
                Pic = BuildUrl(w.LifePhoto),
                Path = $"/writerinfo/{w.Id}"
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<RecommendResponseDto> GetRecommendBooksAsync()
        {
            // 对应 get_recommend_books()

            async Task<List<Book>> QueryBooksAsync(string readerType)
            {
                return await _db.Books
                    .Include(b => b.User)
                    .Where(b => b.ReaderType == readerType)
                    .ToListAsync();
            }

            List<BookOut> Convert(List<Book> books)
            {
                return books.Select(b => new BookOut
                {
                    Id = b.Id,
                    Title = b.Title,
                    Desc = b.Intro ?? "",
                    CoverUrl = BuildUrl(b.CoverUrl),
                    AuthorNickname = b.User?.Nickname ?? "",
                    Path = $"/bookinfo/{b.Id}"
                }).ToList();
            }

            var maleBooks = await QueryBooksAsync("男生");
            var femaleBooks = await QueryBooksAsync("女生");

            List<Book> Sample(List<Book> source)
            {
                var count = Math.Min(5, source.Count);
                return source.OrderBy(_ => _random.Next()).Take(count).ToList();
            }

            var maleSample = Sample(maleBooks);
            var femaleSample = Sample(femaleBooks);

            return new RecommendResponseDto
            {
                Male = Convert(maleSample),
                Female = Convert(femaleSample)
            };
        }

        /// <inheritdoc />
        public async Task<AdaptListResponseDto> GetAdaptListAsync(int? limit)
        {
            // books = db.session.query(Book).options(selectinload(Book.author)).limit(limit).all()
            var query = _db.Books
                .Include(b => b.User)
                .AsQueryable();

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var books = await query.ToListAsync();

            var list = books.Select(book => new AdaptBookOut
            {
                Id = book.Id,
                Pic = BuildUrl(book.CoverUrl),
                Path = $"/bookinfo/{book.Id}"
            }).ToList();

            return new AdaptListResponseDto
            {
                Data = list
            };
        }

        /// <inheritdoc />
        public async Task<BookRankingResponseDto> GetRankingListAsync(string readerType, string plotType)
        {
            // query = db.session.query(Book).filter(Book.reader_type == reader_type, Book.plot_type == plot_type)
            var query = _db.Books
                .Include(b => b.User)
                .Where(b => b.ReaderType == readerType && b.PlotType == plotType);

            var readBooks = await query
                .OrderByDescending(b => b.FavoriteCount)
                .Take(10)
                .ToListAsync();

            var newBooks = await query
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .ToListAsync();

            List<RankingBookOut> Convert(List<Book> books)
            {
                var list = new List<RankingBookOut>();

                for (int i = 0; i < books.Count; i++)
                {
                    var book = books[i];
                    var intro = book.Intro ?? "";
                    var desc = intro.Length > 35
                        ? intro.Substring(0, 35) + "..."
                        : intro;

                    list.Add(new RankingBookOut
                    {
                        Num = (i + 1).ToString("D2"),
                        Title = book.Title,
                        Desc = desc,
                        Path = $"/bookinfo/{book.Id}",
                        Pic = BuildUrl(book.CoverUrl),
                        Author = book.User?.Nickname ?? ""
                    });
                }

                return list;
            }

            return new BookRankingResponseDto
            {
                PlotType = plotType,
                Child = Convert(readBooks),
                NewChild = Convert(newBooks)
            };
        }

        /// <inheritdoc />
        public async Task<List<RecentUpdateItemResponseDto>> GetRecentUpdatesAsync(int limit = 10)
        {
            // Python:
            // db.session.query(Chapter)
            //   .join(Volume).join(Book)
            //   .options(joinedload(Chapter.volume).joinedload(Volume.book).joinedload(Book.author))
            //   .order_by(Chapter.updated_at.desc()).limit(limit)

            var chapters = await _db.Chapters
                .Include(c => c.Volume)
                    .ThenInclude(v => v.Book)
                        .ThenInclude(b => b.User)
                .OrderByDescending(c => c.UpdatedAt)
                .Take(limit)
                .ToListAsync();

            var updates = new List<RecentUpdateItemResponseDto>();

            foreach (var chapter in chapters)
            {
                var volume = chapter.Volume;
                var book = volume?.Book;
                var author = book?.User;

                if (book == null)
                    continue;

                var item = new RecentUpdateItemResponseDto
                {
                    Type = book.PlotType ?? "",
                    Title = book.Title,
                    Path = $"/bookinfo/{book.Id}",
                    Chapter = chapter.Title,
                    Author = author?.Nickname ?? "未知作者",
                    Time = chapter.UpdatedAt.ToString("MM-dd HH:mm")
                };

                updates.Add(item);
            }

            return updates;
        }
    }
}
