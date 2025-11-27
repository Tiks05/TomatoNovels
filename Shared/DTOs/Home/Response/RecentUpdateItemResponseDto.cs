using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home.Response
{
    public class RecentUpdateItemResponseDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";

        [JsonPropertyName("chapter")]
        public string Chapter { get; set; } = "";

        [JsonPropertyName("author")]
        public string Author { get; set; } = "";

        [JsonPropertyName("time")]
        public string Time { get; set; } = "";
    }
}
