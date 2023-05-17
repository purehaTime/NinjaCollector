using TelegramService.Interfaces;
using TelegramService.Models;

namespace TelegramService.Services
{
    public class TelegramConfigService : ITelegramConfigService
    {
        private readonly IConfiguration _appConfig;

        private TelegramConfig _cachedConfig;

        public TelegramConfigService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        public TelegramConfig GetTelegramConfig()
        {
            return _cachedConfig ??= new TelegramConfig
            {
                BotToken = Environment.GetEnvironmentVariable("Telegram_BotToken") ??
                            _appConfig.GetSection("Telegram:BotToken").Value,
                AllowedUsersId = GetAdmins()
            };
        }

        private List<long> GetAdmins()
        {
            var admins = Environment.GetEnvironmentVariable("Telegram_Admins") ??
                         _appConfig.GetSection("Telegram:Admins").Value;

            var parsed = admins.Split("|").Select(s =>
            {
                var valid = long.TryParse(s, out var result);
                return valid ? result : -1;
            });

            return parsed.ToList();
        }
    }
}
