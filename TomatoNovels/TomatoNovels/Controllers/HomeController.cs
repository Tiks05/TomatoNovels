using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Home;
using TomatoNovels.Services;

namespace TomatoNovels.Controllers
{
    /// <summary>
    /// 首页相关接口（对应 Flask home_bp）
    /// </summary>
    [Route("api/home")]
    public class HomeController : ApiControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// 首页 Top 榜单
        /// </summary>
        [HttpGet("top-books")]
        public async Task<ActionResult<ApiResponse<List<TopBookOut>>>> GetTopBooks()
        {
            var list = await _homeService.GetTopBooksAsync();
            return Success(list);
        }

        /// <summary>
        /// 资讯列表
        /// </summary>
        [HttpGet("news-list")]
        public async Task<ActionResult<ApiResponse<List<NewsOut>>>> GetNewsList(
            [FromQuery] int limit = 8)
        {
            var list = await _homeService.GetNewsListAsync(limit);
            return Success(list);
        }

        /// <summary>
        /// 推荐作家列表
        /// </summary>
        [HttpGet("writer-list")]
        public async Task<ActionResult<ApiResponse<List<WriterOut>>>> GetWriterList()
        {
            var list = await _homeService.GetWriterListAsync();
            return Success(list);
        }

        /// <summary>
        /// 男/女生推荐书籍
        /// </summary>
        [HttpGet("recommend")]
        public async Task<ActionResult<ApiResponse<RecommendResponse>>> Recommend()
        {
            var resp = await _homeService.GetRecommendBooksAsync();
            return Success(resp);
        }

        /// <summary>
        /// 改编推荐列表
        /// </summary>
        [HttpGet("adaptlist")]
        public async Task<ActionResult<ApiResponse<AdaptListResponse>>> AdaptList(
            [FromQuery] int? limit)
        {
            var resp = await _homeService.GetAdaptListAsync(limit);
            return Success(resp);
        }

        /// <summary>
        /// 排行榜（按读者频道 + 分类）
        /// </summary>
        [HttpGet("ranking")]
        public async Task<ActionResult<ApiResponse<BookRankingOut>>> Ranking(
            [FromQuery(Name = "reader_type")] string readerType,
            [FromQuery(Name = "plot_type")] string plotType)
        {
            var data = await _homeService.GetRankingListAsync(readerType, plotType);
            return Success(data);
        }

        /// <summary>
        /// 最近更新章节
        /// </summary>
        [HttpGet("recent-updates")]
        public async Task<ActionResult<ApiResponse<List<RecentUpdateItem>>>> RecentUpdates(
            [FromQuery] int limit = 10)
        {
            var updates = await _homeService.GetRecentUpdatesAsync(limit);
            return Success(updates);
        }
    }
}
