using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Writer
{
    public class NoticeDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }
}
