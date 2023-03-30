using GrpcHelper;
using Logger;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

namespace MainService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddGrpcHelper(builder.Configuration);
            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            builder.Services.AddCors();
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
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();
            app.MapControllers();
            app.Run();
        }
    }
}