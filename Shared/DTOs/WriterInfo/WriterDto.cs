using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.WriterInfo
{
    public class WriterDto
    {
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = string.Empty;

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = string.Empty;

        [JsonPropertyName("become_author_at")]
        public string BecomeAuthorAt { get; set; } = string.Empty;

        [JsonPropertyName("total_words")]
        public int TotalWords { get; set; }

        [JsonPropertyName("follower_count")]
        public int FollowerCount { get; set; }
    }
}
