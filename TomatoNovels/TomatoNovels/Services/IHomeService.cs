using TomatoNovels.Shared.DTOs.Home;

namespace TomatoNovels.Services
{
    public interface IHomeService
    {
        Task<List<TopBookOut>> GetTopBooksAsync();
        Task<List<NewsOut>> GetNewsListAsync(int limit);
        Task<List<WriterOut>> GetWriterListAsync();
        Task<RecommendResponse> GetRecommendBooksAsync();
        Task<AdaptListResponse> GetAdaptListAsync(int? limit);
        Task<BookRankingOut> GetRankingListAsync(string readerType, string plotType);
        Task<List<RecentUpdateItem>> GetRecentUpdatesAsync(int limit = 10);
    }
}
