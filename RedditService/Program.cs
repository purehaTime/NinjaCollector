using GrpcHelper;
using Logger;
using RedditService.API;
using RedditService.Interfaces;
using RedditService.Services;
using RestSharp;
using Serilog;

namespace RedditService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddControllers();
                builder.Services.AddGrpc();
                builder.Services.AddGrpcHelper(builder.Configuration);

                builder.Services.AddHttpClient(); //direct
                builder.Services.AddHttpClient<IRedditSession, RedditSessionService>(); //typed

                builder.Services.AddScoped<IRedditAsyncClient, RedditAsyncClient>();
                builder.Services.AddScoped<IRedditApiClient, RedditApiClient>();
                builder.Services.AddScoped<IFileDownloadService, FileDownloadService>();
                builder.Services.AddScoped<IParserGalleryService, ParserGalleryService>();

                builder.Services.AddSingleton<IRedditConfig, RedditConfigService>();
                builder.Services.AddSingleton<IRedditSession, RedditSessionService>();

                builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);


                var app = builder.Build();

                app.MapGet("/", () => "Hello World!");



                app.Run();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.WriteLine(err.InnerException?.Message);
            }
        }
    }
}