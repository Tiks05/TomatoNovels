using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.Dto.BookInfo
{
    public class BookContentDto
    {
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("volumes")]
        public IEnumerable<VolumeDto> Volumes { get; set; } = new List<VolumeDto>();
    }

    public class VolumeDto
    {
        [JsonPropertyName("volume_id")]
        public int VolumeId { get; set; }

        [JsonPropertyName("volume_name")]
        public string VolumeName { get; set; } = "";

        [JsonPropertyName("chapters")]
        public List<ChapterDto> Chapters { get; set; } = new();
    }

    public class ChapterDto
    {
        [JsonPropertyName("chapter_id")]
        public int ChapterId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }
    }
}
