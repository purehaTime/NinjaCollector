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

            var app = builder.Build();
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