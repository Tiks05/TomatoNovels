namespace TomatoNovels.Core.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, int expiresInSeconds = 3600);
    }
}
