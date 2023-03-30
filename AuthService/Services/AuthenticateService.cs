using AuthService.Interfaces;
using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IDatabaseServiceClient _dbClient;
        private readonly Serilog.ILogger _logger;
        private readonly IPasswordHasher<string> _passwordHasher;

        public AuthenticateService(IJwtTokenService jwtTokenService, IDatabaseServiceClient dbClient, IPasswordHasher<string> passwordHasher, Serilog.ILogger logger)
        {
            _jwtTokenService = jwtTokenService;
            _dbClient = dbClient;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<bool> ValidateSession(string token)
        {
            var result = await _jwtTokenService.Verify(token);
            return result;
        }

        public async Task<string> ValidateUser(string userName, string password)
        {
            var user = await _dbClient.GetUser(userName);
            if (user.UserName != userName)
            {
                return string.Empty;
            }

            var passwordValidation = _passwordHasher.VerifyHashedPassword(userName, user.Password, password);
            if (passwordValidation == PasswordVerificationResult.Success)
            {
                _logger.Information($"{userName} successful login");
                return _jwtTokenService.GetJwtToken(userName);
            }

            return string.Empty;
        }
    }
}
