using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class RecentUpdateResponse
    {
        [JsonPropertyName("updates")]
        public List<RecentUpdateItem> Updates { get; set; } = new();
    }
}
