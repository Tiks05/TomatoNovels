using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home.Response
{
    public class RecommendResponseDto
    {
        [JsonPropertyName("male")]
        public List<BookOut> Male { get; set; } = new();

        [JsonPropertyName("female")]
        public List<BookOut> Female { get; set; } = new();
    }
}
