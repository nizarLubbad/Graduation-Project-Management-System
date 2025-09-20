using System.Security.Claims;

namespace GPMS.Helpers
{
    public interface IJwtProvider
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
