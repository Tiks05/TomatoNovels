using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Auth.Request
{
    public class LoginByCodeRequestDto
    {
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
    }
}
