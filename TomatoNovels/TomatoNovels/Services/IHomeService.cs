using TomatoNovels.Shared.DTOs.Home.Response;

namespace TomatoNovels.Services
{
    public interface IHomeService
    {
        Task<List<TopBookResponseDto>> GetTopBooksAsync();
        Task<List<NewsResponseDto>> GetNewsListAsync(int limit);
        Task<List<WriterResponseDto>> GetWriterListAsync();
        Task<RecommendResponseDto> GetRecommendBooksAsync();
        Task<AdaptListResponseDto> GetAdaptListAsync(int? limit);
        Task<BookRankingResponseDto> GetRankingListAsync(string readerType, string plotType);
        Task<List<RecentUpdateItemResponseDto>> GetRecentUpdatesAsync(int limit = 10);
    }
}
