using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.BookInfo
{
    // 顶层结构：get_book_header
    public class BookHeaderDto
    {
        [JsonPropertyName("book")]
        public BookHeaderBookDto Book { get; set; } = new();

        [JsonPropertyName("author")]
        public BookHeaderAuthorDto Author { get; set; } = new();
    }

    public class BookHeaderBookDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = "";

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("tags")]
        public string Tags { get; set; } = "";

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = "";

        [JsonPropertyName("latest_chapter")]
        public int LatestChapter { get; set; }

        [JsonPropertyName("latest_chapter_title")]
        public string LatestChapterTitle { get; set; } = "";
    }

    public class BookHeaderAuthorDto
    {
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = "";

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = "";

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }

    // get_book_content
    public class BookContentDto
    {
        [JsonPropertyName("intro")]
        public string Intro { get; set; } = "";

        [JsonPropertyName("volumes")]
        public List<BookVolumeDto> Volumes { get; set; } = new();
    }

    public class BookVolumeDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("chapter_count")]
        public int ChapterCount { get; set; }

        [JsonPropertyName("chapters")]
        public List<BookChapterItemDto> Chapters { get; set; } = new();
    }

    public class BookChapterItemDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }

    // 阅读页 ChapterReadResponse
    public class ChapterReadResponseDto
    {
        [JsonPropertyName("book_title")]
        public string BookTitle { get; set; } = "";

        [JsonPropertyName("chapter_title")]
        public string ChapterTitle { get; set; } = "";

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("chapter_index")]
        public int ChapterIndex { get; set; }

        [JsonPropertyName("prev_chapter_id")]
        public int? PrevChapterId { get; set; }

        [JsonPropertyName("next_chapter_id")]
        public int? NextChapterId { get; set; }
    }
}
