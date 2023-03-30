using AuthService.Interfaces;
using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Identity;
using ILogger = Serilog.ILogger;

namespace AuthService.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IDatabaseServiceClient _dbClient;
        private readonly ILogger _logger;
        private readonly IInviteService _inviteService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<string> _passwordHasher;

        public AuthorizeService(IDatabaseServiceClient dbClient, ILogger logger, IInviteService inviteService, IJwtTokenService jwtTokenService, IPasswordHasher<string> passwordHasher)
        {
            _dbClient = dbClient;
            _logger = logger;
            _inviteService = inviteService;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> AuthorizeUser(string userName, string password, string invite)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(invite))
                return null;

            var user = await _dbClient.GetUser(userName);
            if (user.UserName == null || !_inviteService.ValidateInvite(invite) || user.UserName == userName)
            {
                return null;
            }

            var hashPass = _passwordHasher.HashPassword(userName, password);
            var result = await _dbClient.CreateUser(userName, hashPass);

            if (result)
            {
                _logger.Information($"Success create user {userName}");
                return _jwtTokenService.GetJwtToken(userName);
            }

            return null;
        }
    }
}
