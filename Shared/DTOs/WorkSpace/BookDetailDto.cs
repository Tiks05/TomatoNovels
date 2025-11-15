using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 作家后台-作品详情（对应 BookDetailSchema）
    /// </summary>
    public class BookDetailDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = string.Empty;

        [JsonPropertyName("target_readers")]
        public string TargetReaders { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public string Tags { get; set; } = string.Empty;

        [JsonPropertyName("main_roles")]
        public string MainRoles { get; set; } = string.Empty;

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("contract_status")]
        public string ContractStatus { get; set; } = string.Empty;

        [JsonPropertyName("update_status")]
        public string UpdateStatus { get; set; } = string.Empty;
    }
}
