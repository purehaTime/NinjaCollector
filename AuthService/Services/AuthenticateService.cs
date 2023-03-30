using AuthService.Interfaces;
using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IDatabaseServiceClient _dbClient;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly IPasswordHasher<string> _passwordHasher;

        public AuthenticateService(IJwtTokenService jwtTokenService, IDatabaseServiceClient dbClient, IConfiguration config, IPasswordHasher<string> passwordHasher)
        {
            _jwtTokenService = jwtTokenService;
            _dbClient = dbClient;
            _config = config;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ValidateSession(string token)
        {
            var result = await _jwtTokenService.Verify(token);
            return result;
        }

        public async Task<string> ValidateUser(string userName, string password)
        {
            var user = await _dbClient.GetUser(userName);
            var passwordValidation = _passwordHasher.VerifyHashedPassword(userName, user.Password, password);

            string token = null;
            if (passwordValidation == PasswordVerificationResult.Success)
            {
                token = _jwtTokenService.GetJwtToken(userName);
            }

            return token;
        }
    }
}
