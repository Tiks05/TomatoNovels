using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 分卷信息（下拉框）（对应 VolumeItem）
    /// </summary>
    public class VolumeItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;
    }

    /// <summary>
    /// 章节列表项（对应 ChapterItem）
    /// </summary>
    public class ChapterItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("volume_id")]
        public int VolumeId { get; set; }

        [JsonPropertyName("chapter_num")]
        public int ChapterNum { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("status_text")]
        public string? StatusText { get; set; }

        [JsonPropertyName("typo_count")]
        public int? TypoCount { get; set; } = 0;
    }

    /// <summary>
    /// 章节列表返回结构（对应 ChapterListResponse）
    /// </summary>
    public class ChapterListResponseDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("volumes")]
        public List<VolumeItemDto> Volumes { get; set; } = new();

        [JsonPropertyName("list")]
        public List<ChapterItemDto> List { get; set; } = new();
    }
}
