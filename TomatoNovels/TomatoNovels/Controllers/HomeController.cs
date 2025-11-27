using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Services;
using TomatoNovels.Shared.DTOs.Home.Request;
using TomatoNovels.Shared.DTOs.Home.Response;

namespace TomatoNovels.Controllers
{
    /// <summary>
    /// 首页相关接口（对应 Flask home_bp）
    /// </summary>
    [Route("api/home1")]
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
        public async Task<ActionResult<ApiResponse<List<TopBookResponseDto>>>> GetTopBooks(
            [FromQuery] TopBooksRequestDto request)
        {
            // 当前没有筛选条件，所以暂时不使用 request
            var list = await _homeService.GetTopBooksAsync();
            return Success(list);
        }

        /// <summary>
        /// 资讯列表
        /// </summary>
        [HttpGet("news-list")]
        public async Task<ActionResult<ApiResponse<List<NewsResponseDto>>>> GetNewsList(
            [FromQuery] NewsListRequestDto request)
        {
            var limit = request.Limit ?? 8;
            var list = await _homeService.GetNewsListAsync(limit);
            return Success(list);
        }

        /// <summary>
        /// 推荐作家列表
        /// </summary>
        [HttpGet("writer-list")]
        public async Task<ActionResult<ApiResponse<List<WriterResponseDto>>>> GetWriterList(
            [FromQuery] WriterListRequestDto request)
        {
            // 当前没有过滤条件，request 可以先为空 DTO
            var list = await _homeService.GetWriterListAsync();
            return Success(list);
        }

        /// <summary>
        /// 男/女生推荐书籍
        /// </summary>
        [HttpGet("recommend")]
        public async Task<ActionResult<ApiResponse<RecommendResponseDto>>> Recommend(
            [FromQuery] RecommendRequestDto request)
        {
            // 如果后面要按 readerType 等筛选，可以往 request 里加字段
            var resp = await _homeService.GetRecommendBooksAsync();
            return Success(resp);
        }

        /// <summary>
        /// 改编推荐列表
        /// </summary>
        [HttpGet("adaptlist")]
        public async Task<ActionResult<ApiResponse<AdaptListResponseDto>>> AdaptList(
            [FromQuery] AdaptListRequestDto request)
        {
            var resp = await _homeService.GetAdaptListAsync(request.Limit);
            return Success(resp);
        }

        /// <summary>
        /// 排行榜（按读者频道 + 分类）
        /// </summary>
        [HttpGet("ranking")]
        public async Task<ActionResult<ApiResponse<BookRankingResponseDto>>> Ranking(
            [FromQuery] RankingRequestDto request)
        {
            var data = await _homeService.GetRankingListAsync(
                request.ReaderType,
                request.PlotType
            );
            return Success(data);
        }

        /// <summary>
        /// 最近更新章节
        /// </summary>
        [HttpGet("recent-updates")]
        public async Task<ActionResult<ApiResponse<List<RecentUpdateItemResponseDto>>>> RecentUpdates(
            [FromQuery] RecentUpdatesRequestDto request)
        {
            var limit = request.Limit ?? 10;
            var updates = await _homeService.GetRecentUpdatesAsync(limit);
            return Success(updates);
        }
    }
}
