using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Writer
{
    public class PicNoticeDto
    {
        [JsonPropertyName("cover_url")]
        public string? CoverUrl { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }
}
