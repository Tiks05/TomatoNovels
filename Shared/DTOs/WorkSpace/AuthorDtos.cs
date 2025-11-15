using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 作者申请表单（对应 AuthorApplyForm）
    /// </summary>
    public class AuthorApplyFormDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("introduction")]
        public string Introduction { get; set; } = string.Empty;
    }

    /// <summary>
    /// 作者申请结果（对应 AuthorApplyResult）
    /// </summary>
    public class AuthorApplyResultDto
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;

        [JsonPropertyName("become_author_at")]
        public string? BecomeAuthorAt { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = string.Empty;
    }

    /// <summary>
    /// 作家统计信息（对应 AuthorStatsSchema）
    /// </summary>
    public class AuthorStatsDto
    {
        [JsonPropertyName("fans_count")]
        public int FansCount { get; set; }

        [JsonPropertyName("total_words")]
        public int TotalWords { get; set; }
    }
}
