using GrpcHelper;
using Logger;
using Serilog;
using Telegram.Bot;
using TelegramService.API;
using TelegramService.Interfaces;
using TelegramService.Services;
using TelegramService.Workers;
using Worker.ServiceExtension;

namespace TelegramService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient("TelegramBot").AddTypedClient<string>()
                .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
                {
                    var configService = serviceProvider.GetRequiredService<ITelegramConfigService>();
                    var botConfig = configService.GetTelegramConfig();
                    var options = new TelegramBotClientOptions(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });


            builder.Services.AddScoped<ITelegramBotApiClient, TelegramBotApiClient>();
            builder.Services.AddScoped<ITelegramBotService, TelegramBotService>();
            builder.Services.AddSingleton<ITelegramConfigService, TelegramConfigService>();
            builder.Services.AddGrpcHelper(builder.Configuration);
            builder.Services.AddWorker<PosterWorker>();

            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);

            var app = builder.Build();

            app.MapWorkerService();
            await app.RunWorkers();
            await app.RunAsync();
        }
    }
}