using TomatoNovels.Client.ApiRequest;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Home.Request;
using TomatoNovels.Shared.DTOs.Home.Response;

namespace TomatoNovels.Client.Services
{
    public class HomeApi
    {
        private readonly ApiRequest.ApiRequest _http;

        public HomeApi(ApiRequest.ApiRequest http)
        {
            _http = http;
        }

        // =========================
        // 首页排行榜（Top Books）
        // GET /home/top-books
        // =========================
        public Task<ApiResponse<List<TopBookResponseDto>>> FetchTopBooksAsync()
        {
            // 即便现在没参数，也统一用 RequestDto，方便以后扩展
            var req = new TopBooksRequestDto();

            return _http.GetAsync<List<TopBookResponseDto>>("home/top-books");
        }

        // =========================
        // 首页最新资讯
        // GET /home/news-list?limit=xx
        // =========================
        public Task<ApiResponse<List<NewsResponseDto>>> FetchNewsListAsync(int? limit = null)
        {
            var req = new NewsListRequestDto
            {
                Limit = limit
            };

            var url = req.Limit is null
                ? "home/news-list"
                : $"home/news-list?limit={req.Limit}";

            return _http.GetAsync<List<NewsResponseDto>>(url);
        }

        // =========================
        // 首页推荐作家列表
        // GET /home/writer-list
        // =========================
        public Task<ApiResponse<List<WriterResponseDto>>> GetWriterListAsync()
        {
            var req = new WriterListRequestDto(); // 目前为空壳，将来可扩展

            return _http.GetAsync<List<WriterResponseDto>>("home/writer-list");
        }

        // =========================
        // 推荐书籍（男生/女生）
        // GET /home/recommend
        // =========================
        public Task<ApiResponse<RecommendResponseDto>> GetRecommendBooksAsync()
        {
            var req = new RecommendRequestDto(); // 以后可以加 ReaderType 等字段

            return _http.GetAsync<RecommendResponseDto>("home/recommend");
        }

        // =========================
        // 版权改编列表
        // GET /home/adaptlist?limit=xx
        // =========================
        public Task<ApiResponse<AdaptListResponseDto>> GetAdaptListAsync(int? limit = null)
        {
            var req = new AdaptListRequestDto
            {
                Limit = limit
            };

            var url = req.Limit is null
                ? "home/adaptlist"
                : $"home/adaptlist?limit={req.Limit}";

            return _http.GetAsync<AdaptListResponseDto>(url);
        }

        // =========================
        // 获取排行榜（按 reader_type + plot_type）
        // GET /home/ranking?reader_type=xx&plot_type=yy
        // =========================
        public Task<ApiResponse<BookRankingResponseDto>> FetchRankingListAsync(
            string readerType,
            string plotType)
        {
            var req = new RankingRequestDto
            {
                ReaderType = readerType,
                PlotType = plotType
            };

            var url =
                $"home/ranking?reader_type={req.ReaderType}&plot_type={req.PlotType}";

            return _http.GetAsync<BookRankingResponseDto>(url);
        }

        // =========================
        // 首页最近更新章节
        // GET /home/recent-updates?limit=xx
        // =========================
        public Task<ApiResponse<List<RecentUpdateItemResponseDto>>> GetRecentUpdatesAsync(
            int? limit = null)
        {
            var req = new RecentUpdatesRequestDto
            {
                Limit = limit
            };

            var url = req.Limit is null
                ? "home/recent-updates"
                : $"home/recent-updates?limit={req.Limit}";

            return _http.GetAsync<List<RecentUpdateItemResponseDto>>(url);
        }
    }
}
