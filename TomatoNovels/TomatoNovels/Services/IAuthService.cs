using TomatoNovels.Models;

namespace TomatoNovels.Services
{
    /// <summary>
    /// 认证相关业务接口（登录 / 注册）
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 登录或注册
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="password">密码明文</param>
        /// <returns>用户实体和 JWT Token</returns>
        Task<(User user, string token)> LoginOrRegisterAsync(string phone, string password);
    }
}
