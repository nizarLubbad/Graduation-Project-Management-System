namespace GPMS.Helpers
{
    public interface IJwtProvider
    {
        string GenerateToken(int userId, string role);
    }
}
