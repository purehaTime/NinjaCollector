using System.IdentityModel.Tokens.Jwt;
using AuthService.Interfaces;

namespace AuthService.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private JwtSecurityTokenHandler tokenhandler;

        public string GetJwtToken(string userName, string password)
        {
            //throw new NotImplementedException();

            tokenhandler.CreateJwtSecurityToken()
        }
    }
}
