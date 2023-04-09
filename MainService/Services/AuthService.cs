using GrpcHelper.AuthService;
using GrpcHelper.Interfaces;
using MainService.Interfaces;

namespace MainService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthServiceClient _authClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(IAuthServiceClient authClient, IHttpContextAccessor contextAccessor)
        {
            _authClient = authClient;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Login(string userLogin, string password)
        {
            var result = await _authClient.Authenticate(new AuthenticateModel
            {
                UserLogin = userLogin,
                UserPassword = password
            });

            if (!string.IsNullOrEmpty(result))
            {
                _contextAccessor.HttpContext?.Response.Cookies.Append("session", result, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(5)
                });
                return true;
            }

            return false;
        }

        public async Task<bool> Register(string userLogin, string password, string invite)
        {
            var result = await _authClient.Authorize(new AuthorizeModel
            {
                Invite = invite,
                UserLogin = userLogin,
                UserPassword = password
            });

            if (!string.IsNullOrEmpty(result))
            {
                _contextAccessor.HttpContext?.Response.Cookies.Append("session", result);
                return true;
            }

            return false;
        }

        public Task<bool> Logout()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete("session");
            return Task.FromResult(true);
        }

        public async Task<bool> Verify(string token)
        {
            return await _authClient.Verify(token);
        }
    }
}
