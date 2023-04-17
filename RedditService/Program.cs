using System.ComponentModel.DataAnnotations;
using GrpcHelper;
using Logger;
using RedditService.API;
using RedditService.Interfaces;
using RedditService.Services;
using RedditService.Workers;
using Serilog;
using Worker;
using Worker.ServiceExtension;

namespace RedditService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddGrpcHelper(builder.Configuration);

                builder.Services.AddHttpClient(); //direct
                builder.Services.AddHttpClient<IRedditSession, RedditSessionService>(); //typed

                builder.Services.AddScoped<IRedditAsyncClient, RedditAsyncClient>();
                builder.Services.AddScoped<IRedditApiClient, RedditApiClient>();
                builder.Services.AddScoped<IFileDownloadService, FileDownloadService>();
                builder.Services.AddScoped<IParserGalleryService, ParserGalleryService>();
                builder.Services.AddScoped<IRedditService, Services.RedditService>();
                builder.Services.AddScoped<IParserService, ParserService>();
                builder.Services.AddScoped<IFilterService, FilterService>();

                builder.Services.AddSingleton<IRedditConfig, RedditConfigService>();
                builder.Services.AddSingleton<IRedditSession, RedditSessionService>();

                builder.Services.AddWorker<ParserWorker>();

                builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);

                var app = builder.Build();

                app.MapGet("/", () => "Hello World!");

                app.MapWorkerService();
                await app.RunWorkers();

                await app.RunAsync();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.WriteLine(err.InnerException?.Message);
            }
        }
    }
}