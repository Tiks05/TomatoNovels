using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.WriterInfo
{
    public class WorkDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("tags")]
        public string Tags { get; set; } = string.Empty;

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = string.Empty;

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }

        [JsonPropertyName("bookinfo_path")]
        public string BookInfoPath { get; set; } = string.Empty;

        [JsonPropertyName("max_chapter")]
        public int? MaxChapter { get; set; }

        [JsonPropertyName("max_chapter_title")]
        public string? MaxChapterTitle { get; set; }
    }
}
