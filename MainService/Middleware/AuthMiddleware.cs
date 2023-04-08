namespace MainService.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path != "/Auth")
            {
                if (!context.Request.Cookies.TryGetValue("session", out var token))
                {
                    context.Response.Redirect("/Auth");
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
