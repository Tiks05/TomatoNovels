using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Auth.Request
{
    public class SendCodeRequestDto
    {
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
    }
}
