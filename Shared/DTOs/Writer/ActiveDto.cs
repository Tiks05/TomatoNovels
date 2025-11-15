using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Writer
{
    public class ActiveDto
    {
        [JsonPropertyName("cover_url")]
        public string? CoverUrl { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 格式化后的日期字符串，例如 "2025-01-01"
        /// </summary>
        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}
