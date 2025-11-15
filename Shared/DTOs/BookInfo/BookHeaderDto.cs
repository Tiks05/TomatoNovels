using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.Dto.BookInfo
{
    public class BookHeaderDto
    {
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; } = "";

        [JsonPropertyName("cover")]
        public string Cover { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("chapter_count")]
        public int ChapterCount { get; set; }
    }
}
