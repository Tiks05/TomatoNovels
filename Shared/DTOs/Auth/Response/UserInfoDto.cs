using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Auth.Response
{
    public class UserInfoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = "guest";

        [JsonPropertyName("nickname")]
        public string? Nickname { get; set; }

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        [JsonPropertyName("become_author_at")]
        public string? BecomeAuthorAt { get; set; }

        [JsonPropertyName("signature")]
        public string? Signature { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
}
