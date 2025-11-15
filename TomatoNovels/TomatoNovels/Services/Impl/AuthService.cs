using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TomatoNovels.Controllers.Auth;
using TomatoNovels.Controllers.Exceptions;
using TomatoNovels.Data;
using TomatoNovels.Models;
using TomatoNovels.Utils;

namespace TomatoNovels.Services.Impl
{
    /// <summary>
    /// 登录/注册 业务实现
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AppDbContext db,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthService> logger)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<(User user, string token)> LoginOrRegisterAsync(string phone, string password)
        {
            // 对应 Flask: user = db.session.query(User).filter_by(phone=phone).first()
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Phone == phone);

            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    throw new ApiException("该账号未设置密码", 40002);
                }

                if (!PasswordUtils.CheckPassword(password, user.Password))
                {
                    throw new ApiException("账号或密码错误，请重试", 40003);
                }
            }
            else
            {
                // hashed = hash_password(password)
                var hashed = PasswordUtils.HashPassword(password);

                // user = User(...)
                user = new User
                {
                    Phone = phone,
                    Password = hashed,
                    Role = "user",
                    Nickname = phone.Length >= 3 ? phone[..3] + "****" : phone + "****",
                    Avatar = "/wwwroot/assets/avatars/icons8-user-pulsar-color-32.png"
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                _logger.LogInformation("新用户注册成功：{Phone}", phone);
            }

            // token = generate_jwt(user.id)
            var token = _jwtTokenGenerator.GenerateToken(user.Id);

            return (user, token);
        }
    }
}
