using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Layout
{
    public class UserProfileUpdateResultDto
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = "";

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = "";

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = "";
    }
}
