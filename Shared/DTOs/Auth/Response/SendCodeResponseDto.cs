using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Auth.Response
{
    public class SendCodeResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
