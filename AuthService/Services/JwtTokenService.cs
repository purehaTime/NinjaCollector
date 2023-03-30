using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        private readonly string _key;
        private readonly string _expires;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
            _key = Environment.GetEnvironmentVariable("Security_Key") ?? _config["Security:Key"];
            _expires = Environment.GetEnvironmentVariable("Security_Expires") ?? _config["Security:Expires"];
            _issuer = Environment.GetEnvironmentVariable("Security_Issuer") ?? _config["Security:Issuer"];
            _audience = Environment.GetEnvironmentVariable("Security_Audience") ?? _config["Security:Audience"];
        }

        public string GetJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var expiresMinutes = double.TryParse(_expires, out var parsedMinutes) ? parsedMinutes : 1;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userName", userName) }),
                Expires = DateTime.UtcNow.AddMinutes(expiresMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> Verify(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidIssuer = _issuer,
                ClockSkew = TimeSpan.Zero
            });
            return result.IsValid;
        }
    }
}
