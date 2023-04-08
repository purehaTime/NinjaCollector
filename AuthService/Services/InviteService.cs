using AuthService.Interfaces;

namespace AuthService.Services
{
    public class InviteService : IInviteService
    {
        private readonly IConfiguration _config;

        public InviteService(IConfiguration config)
        {
            _config = config;
        }

        public bool ValidateInvite(string invite)
        {
            var invites = Environment.GetEnvironmentVariable("Invites")?.Split("|") ??
                          _config.GetSection("invites").GetChildren().Select(s => s.Value);
            var key = invites.FirstOrDefault(f => f == invite);

            return !string.IsNullOrEmpty(key);
        }
    }
}
