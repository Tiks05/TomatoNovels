using BCrypt.Net;

namespace TomatoNovels.Utils
{
    public static class PasswordUtils
    {
        /// <summary>
        /// 使用 BCrypt 生成哈希（等价于 Python bcrypt.hashpw）
        /// </summary>
        public static string HashPassword(string plain)
        {
            return BCrypt.Net.BCrypt.HashPassword(plain);
        }

        /// <summary>
        /// 校验密码（等价于 Python bcrypt.checkpw）
        /// </summary>
        public static bool CheckPassword(string plain, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(plain, hashed);
        }
    }
}
