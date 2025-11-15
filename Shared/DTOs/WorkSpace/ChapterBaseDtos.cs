using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 最近一章信息（用于自动续写）（对应 ChapterInfoSchema）
    /// </summary>
    public class ChapterInfoDto
    {
        [JsonPropertyName("volume_index")]
        public int? VolumeIndex { get; set; } = 0;

        [JsonPropertyName("volume_title")]
        public string? VolumeTitle { get; set; } = string.Empty;

        [JsonPropertyName("chapter_index")]
        public int? ChapterIndex { get; set; } = 0;

        [JsonPropertyName("chapter_title")]
        public string? ChapterTitle { get; set; } = string.Empty;
    }

    /// <summary>
    /// 创建章节（对应 ChapterCreateSchema）
    /// </summary>
    public class ChapterCreateDto
    {
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("volume_id")]
        public int? VolumeId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }
    }

    /// <summary>
    /// 更新章节（对应 ChapterUpdateSchema）
    /// </summary>
    public class ChapterUpdateDto
    {
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("chapter_id")]
        public int ChapterId { get; set; }

        [JsonPropertyName("chapter_num")]
        public int ChapterNum { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("is_draft")]
        public bool? IsDraft { get; set; } = false;
    }

    /// <summary>
    /// 章节详情（对应 ChapterDetailSchema）
    /// </summary>
    public class ChapterDetailDto
    {
        [JsonPropertyName("volume_index")]
        public int VolumeIndex { get; set; }

        [JsonPropertyName("volume_title")]
        public string VolumeTitle { get; set; } = string.Empty;

        [JsonPropertyName("chapter_num")]
        public int ChapterNum { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 分卷 + 章节查询（对应 VolumeChapterQuerySchema）
    /// </summary>
    public class VolumeChapterQueryDto
    {
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("volume_id")]
        public int VolumeId { get; set; }
    }
}
