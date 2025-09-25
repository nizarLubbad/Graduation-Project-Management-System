using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GPMS.Models;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Helpers
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;

        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = _config["JwtConfig:Key"];
            var issuer = _config["JwtConfig:Issuer"];
            var audience = _config["JwtConfig:Audience"];
            var expiryMinutes = int.Parse(_config["JwtConfig:TokenValidityMins"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
