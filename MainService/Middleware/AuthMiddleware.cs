using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthServiceClient authClient)
        {
            if (context.Request.Path != "/Auth")
            {
                context.Request.Cookies.TryGetValue("session", out var token);
                if (token == null )
                {
                    context.Response.Redirect("/Auth");
                    return;
                }

                var isValidToken = await authClient.Verify(token ?? "");
                if (!isValidToken)
                {
                    context.Response.Redirect("/Auth");
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class AuthMiddlewareHelper
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<AuthMiddleware>();
    }
}
