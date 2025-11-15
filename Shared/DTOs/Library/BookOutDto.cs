using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Library
{
    /// <summary>
    /// 书籍列表单项（对应 BookOutSchema）
    /// </summary>
    public class BookOutDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = string.Empty;

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = string.Empty;

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;

        /// <summary>
        /// 阅读页路径，例如 /read/123/xxx
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }
}
