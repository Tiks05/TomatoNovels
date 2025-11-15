using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Auth.Response
{
    public class LoginOrRegisterResponseDto
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [JsonPropertyName("user")]
        public UserInfoDto User { get; set; } = new UserInfoDto();

        /// <summary>
        /// JWT Token
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
