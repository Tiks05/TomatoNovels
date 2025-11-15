using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Layout
{
    public class SearchBookItemDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("author")]
        public string Author { get; set; } = "";

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("wordCount")]
        public int WordCount { get; set; }

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = "";

        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; } = "";

        [JsonPropertyName("pic")]
        public string Pic { get; set; } = "";

        [JsonPropertyName("people")]
        public int People { get; set; }

        [JsonPropertyName("update")]
        public string Update { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";

        [JsonPropertyName("readPath")]
        public string ReadPath { get; set; } = "";

        [JsonPropertyName("updatePath")]
        public string UpdatePath { get; set; } = "";
    }
}
