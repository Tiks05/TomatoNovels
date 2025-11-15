using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 最近章节信息（用于各种“继续上一章写”逻辑）（对应 LastChapterResponse）
    /// </summary>
    public class LastChapterResponseDto
    {
        [JsonPropertyName("volume_title")]
        public string? VolumeTitle { get; set; }

        [JsonPropertyName("current_volume_id")]
        public int? CurrentVolumeId { get; set; }

        [JsonPropertyName("last_volume_id")]
        public int LastVolumeId { get; set; }

        [JsonPropertyName("last_volume_title")]
        public string LastVolumeTitle { get; set; } = string.Empty;

        [JsonPropertyName("chapter_index")]
        public int ChapterIndex { get; set; }

        [JsonPropertyName("chapter_title")]
        public string? ChapterTitle { get; set; } = string.Empty;

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; } = string.Empty;
    }

    /// <summary>
    /// 最新章节信息（对应 LatestChapterResponse）
    /// </summary>
    public class LatestChapterResponseDto
    {
        [JsonPropertyName("latest_volume_sort")]
        public int LatestVolumeSort { get; set; }

        [JsonPropertyName("latest_chapter_num")]
        public int LatestChapterNum { get; set; }

        [JsonPropertyName("latest_chapter_title")]
        public string LatestChapterTitle { get; set; } = string.Empty;

        [JsonPropertyName("latest_chapter_updated_at")]
        public string? LatestChapterUpdatedAt { get; set; } = string.Empty;
    }
}
