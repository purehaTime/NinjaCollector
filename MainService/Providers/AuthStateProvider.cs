using System.Security.Claims;
using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainService.Providers;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly IAuthServiceClient _authClient;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthStateProvider(IAuthServiceClient authClient, IHttpContextAccessor contextAccessor)
    {
        _authClient = authClient;
        _contextAccessor = contextAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var cookies = _contextAccessor.HttpContext?.Request.Cookies;
        var claimsIdentity = new ClaimsIdentity();
        if (cookies?.TryGetValue("session", out var token) ?? false)
        {
            var valid = await _authClient.Verify(token);
            if (valid)
            {
                claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "user"),
                    new Claim(ClaimTypes.Role, "role"),
                    new Claim("Jti", Guid.NewGuid().ToString()),
                }, "customAuth" );
            }
        }

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return new AuthenticationState(claimsPrincipal);
    }
}