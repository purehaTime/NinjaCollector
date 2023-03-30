using GrpcHelper.AuthService;

namespace GrpcHelper.Interfaces
{
    public interface IAuthServiceClient
    {
        public Task<string> Authorize(AuthorizeModel authModel);
        public Task<string> Authenticate(AuthenticateModel authModel);
        public Task<bool> Verify(string token);
    }
}
