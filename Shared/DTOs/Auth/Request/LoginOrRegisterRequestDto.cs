using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Auth.Request
{
    public class LoginOrRegisterRequestDto
    {
        /// <summary>
        /// 用户手机号
        /// </summary>
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
