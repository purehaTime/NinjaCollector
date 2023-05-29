using GrpcHelper;
using Logger;
using MainService.Interfaces;
using MainService.Middleware;
using MainService.Models;
using MainService.Providers;
using MainService.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

namespace MainService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpcHelper(builder.Configuration);
            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IParserService, ParserService>();
            builder.Services.AddScoped<IPosterService, PosterService>();
            builder.Services.AddScoped<IWorkerService<RedditStatusModel>, RedditStatusService>();
            builder.Services.AddScoped<IWorkerService<TelegramStatusModel>, TelegramStatusService>();

            builder.Services.AddAntiforgery(opt => {
                opt.Cookie.Name = "x-xsrf-token";
                opt.Cookie.Expiration = TimeSpan.FromDays(1);
            });

            var app = builder.Build();

            app.UseCors(x => x
                .WithOrigins("https://localhost:443")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthMiddleware();

            app.UseRouting();

            app.MapRazorPages();
            app.MapBlazorHub();

            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}