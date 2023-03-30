using AuthService.Interfaces;
using Grpc.Core;
using GrpcHelper.AuthService;
using Status = GrpcHelper.AuthService.Status;

namespace AuthService.Services
{
    public class AuthService : Auth.AuthBase
    {
        private readonly IAuthorizeService _authorize;
        private readonly IAuthenticateService _authenticate;

        public AuthService(IAuthorizeService authorize, IAuthenticateService authenticate)
        {
            _authorize = authorize;
            _authenticate = authenticate;
        }

        public override async Task<TokenModel> Authorize(AuthorizeModel request, ServerCallContext context)
        {
            var token = await _authorize.AuthorizeUser(request.UserLogin, request.UserPassword, request.Invite);
            return new TokenModel { Jwt = token ?? string.Empty };
        }

        public override async Task<TokenModel> Authenticate(AuthenticateModel request, ServerCallContext context)
        {
            var token = await _authenticate.ValidateUser(request.UserLogin, request.UserPassword);
            return new TokenModel { Jwt = token ?? string.Empty };
        }

        public override async Task<Status> Validate(TokenModel request, ServerCallContext context)
        {
            var result = await _authenticate.ValidateSession(request.Jwt);
            return new Status { UserAuth = result };
        }
    }
}
