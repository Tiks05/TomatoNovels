using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Services;
using TomatoNovels.Shared.DTOs.Auth.Request;
using TomatoNovels.Shared.DTOs.Auth.Response;

namespace TomatoNovels.Controllers
{
    [Route("api/auth/login_or_register")]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 用户登录或注册（POST /api/auth/pwd）
        /// </summary>
        [HttpPost("pwd")]
        public async Task<ActionResult<ApiResponse<LoginOrRegisterResponseDto>>> LoginOrRegister(
            [FromBody] LoginOrRegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Phone) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return Fail<LoginOrRegisterResponseDto>(
                    code: "Auth.EmptyField",
                    message: "手机号和密码不能为空"
                );
            }

            var (user, token) = await _authService.LoginOrRegisterAsync(dto.Phone, dto.Password);

            if (user == null)
            {
                return Fail<LoginOrRegisterResponseDto>(
                    code: "Auth.LoginFailed",
                    message: "手机号或密码错误"
                );
            }

            // 生成 avatar URL
            var request = _httpContextAccessor.HttpContext!.Request;
            var host = $"{request.Scheme}://{request.Host}";
            var avatarUrl = host.TrimEnd('/') + user.Avatar;

            var result = new LoginOrRegisterResponseDto
            {
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Phone = user.Phone,
                    Role = user.Role,
                    Nickname = user.Nickname,
                    Avatar = avatarUrl,
                    BecomeAuthorAt = user.BecomeAuthorAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Signature = user.Signature,
                    Level = user.Level
                },
                Token = token
            };

            return Success(result);
        }
    }
}
