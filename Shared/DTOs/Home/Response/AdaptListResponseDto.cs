using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home.Response
{
    public class AdaptListResponseDto
    {
        [JsonPropertyName("data")]
        public List<AdaptBookOut> Data { get; set; } = new();
    }
}
