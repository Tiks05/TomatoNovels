using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home.Response
{
    public class NewsResponseDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }
}
