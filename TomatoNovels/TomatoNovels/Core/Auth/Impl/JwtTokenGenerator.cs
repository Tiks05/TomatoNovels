using System;
using System.IdentityModel.Tokens.Jwt; // 需要 System.IdentityModel.Tokens.Jwt 包支持
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TomatoNovels.Core.Auth.Impl
{
    /// <summary>
    /// JWT 生成器，实现等价于 Python 版：
    /// payload = {"user_id": user_id, "exp": now + expires}
    /// token = jwt.encode(payload, SECRET_KEY, algorithm="HS256")
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 生成 JWT Token
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="expiresInSeconds">过期时间（秒），默认 3600 秒</param>
        public string GenerateToken(int userId, int expiresInSeconds = 3600)
        {
            // 等价于 Flask 的 current_app.config["SECRET_KEY"]
            var secret = _configuration["SECRET_KEY"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new InvalidOperationException("SECRET_KEY 未在配置文件中设置。");
            }

            // HS256 对称加密
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // payload: { "user_id": userId, "exp": now + expires }
            var claims = new[]
            {
                new Claim("user_id", userId.ToString())
            };

            var now = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.AddSeconds(expiresInSeconds),
                signingCredentials: credentials
            );

            // 相当于 python 的 jwt.encode(...).decode()
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
