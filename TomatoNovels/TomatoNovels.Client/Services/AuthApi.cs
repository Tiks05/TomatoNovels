using TomatoNovels.Client.ApiRequest;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Auth.Request;
using TomatoNovels.Shared.DTOs.Auth.Response;

namespace TomatoNovels.Client.Services
{
    public class AuthApi
    {
        private readonly ApiRequest.ApiRequest _http;

        public AuthApi(ApiRequest.ApiRequest http)
        {
            _http = http;
        }

        // ================================
        // 发送验证码 auth/login_or_register/sms
        // ================================
        public Task<ApiResponse<SendCodeResponseDto>> SendCode(string phone)
        {
            var body = new SendCodeRequestDto { Phone = phone };
            return _http.PostAsync<SendCodeResponseDto>("auth/login_or_register/sms", body);
        }

        // ================================
        // 验证码登录 auth/login_or_register/code
        // ================================
        public Task<ApiResponse<LoginOrRegisterResponseDto>> LoginByCode(string phone, string code)
        {
            var body = new LoginByCodeRequestDto { Phone = phone, Code = code };
            return _http.PostAsync<LoginOrRegisterResponseDto>("auth/login_or_register/code", body);
        }

        // ================================
        // 密码登录 auth/login_or_register/pwd
        // ================================
        public Task<ApiResponse<LoginOrRegisterResponseDto>> LoginByPassword(string phone, string password)
        {
            var body = new LoginOrRegisterRequestDto
            {
                Phone = phone,
                Password = password
            };

            return _http.PostAsync<LoginOrRegisterResponseDto>("auth/login_or_register/pwd", body);
        }
    }
}
