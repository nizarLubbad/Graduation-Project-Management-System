using GPMS.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GPMS.Services
{
    public class TokenTestService
    {
        private readonly IJwtProvider _jwtProvider;

        public TokenTestService(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public string GenerateTestToken(long studentId = 98, string email = "io@example.com")
        {
            var claims = new List<Claim>
            {
                new Claim("sub", studentId.ToString()),                // NameClaimType في JWT
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, "Student"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ضروري للتوكن الفريد
            };

            return _jwtProvider.GenerateToken(claims);
        }
    }
}
