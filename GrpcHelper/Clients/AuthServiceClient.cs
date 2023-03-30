using GrpcHelper.AuthService;
using GrpcHelper.Interfaces;

namespace GrpcHelper.Clients
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly Auth.AuthClient _client;

        public AuthServiceClient(Auth.AuthClient client)
        {
            _client = client;
        }

        public async Task<string> Authorize(AuthorizeModel authModel)
        {
            var result = await _client.AuthorizeAsync(authModel);
            return result.Jwt;
        }

        public async Task<string> Authenticate(AuthenticateModel authModel)
        {
            var result = await _client.AuthenticateAsync(authModel);
            return result.Jwt;
        }

        public async Task<bool> Verify(string token)
        {
            var result = await _client.ValidateAsync(new TokenModel { Jwt = token });
            return result.UserAuth;
        }
    }
}
