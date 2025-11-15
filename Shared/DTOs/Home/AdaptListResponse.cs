using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class AdaptListResponse
    {
        [JsonPropertyName("data")]
        public List<AdaptBookOut> Data { get; set; } = new();
    }
}
