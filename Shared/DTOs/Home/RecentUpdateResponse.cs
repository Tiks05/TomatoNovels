using System.Text.Json.Serialization;
using TomatoNovels.Shared.DTOs.Home.Response;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class RecentUpdateResponse
    {
        [JsonPropertyName("updates")]
        public List<RecentUpdateItemResponseDto> Updates { get; set; } = new();
    }
}
