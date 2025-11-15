using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class NewsOut
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }
}
