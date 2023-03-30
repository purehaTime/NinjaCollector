using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.Interfaces;
using AuthService.Services;
using GrpcHelper;
using Logger;
using Serilog;
using Microsoft.AspNetCore.Identity;

namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpcHelper(builder.Configuration);
            builder.Services.AddScoped<IInviteService, InviteService>();
            builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
            builder.Services.AddScoped<IAuthorizeService, AuthorizeService>();
            builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
            builder.Services.AddTransient<IPasswordHasher<string>, PasswordHasher<string>>();

            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            /*
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = GetTokenParameters(builder);
            });
            */

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.MapGet("/test", async (IAuthorizeService service) =>
            {
                var result = await service.AuthorizeUser("test", "pass", "invite");
                return result;
            });
            app.MapGrpcService<Services.AuthService>();
            app.Run();
        }

        private static TokenValidationParameters GetTokenParameters(WebApplicationBuilder builder)
        {
            var key = Environment.GetEnvironmentVariable("Security_Key") ?? builder.Configuration["Security:Key"];
            return new TokenValidationParameters
            {
                ValidIssuer = Environment.GetEnvironmentVariable("Security_Issuer") ?? builder.Configuration["Security:Issuer"],
                ValidAudience = Environment.GetEnvironmentVariable("Security_Audience") ?? builder.Configuration["Security:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        }

    }
}