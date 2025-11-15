using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Writer
{
    public class ClassroomOutDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = string.Empty;

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("is_include_video")]
        public bool IsIncludeVideo { get; set; }
    }
}
