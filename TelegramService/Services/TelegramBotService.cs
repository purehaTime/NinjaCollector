using ModelsHelper.Mapping;
using ModelsHelper.Models;
using TelegramService.Interfaces;
using TelegramService.Models;
using ILogger = Serilog.ILogger;

namespace TelegramService.Services
{
    public class TelegramBotService  : ITelegramBotService
    {
        private readonly ITelegramBotApiClient _apiClient;
        private readonly ILogger _logger;

        public TelegramBotService(ITelegramBotApiClient apiClient, ILogger logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<bool> SendPost(string chatId, string message, List<Image> images)
        {
            if (images == null || images.Count == 0)
            {
                return await _apiClient.SendMessage(chatId, message);
            }

            if (images.Count == 1)
            {
                var img = images.First();
                var stream = new MemoryStream(img.File);
                return await _apiClient.SendPicture(chatId, message, stream);
            }

            var gallery = images.Select(s => new MemoryStream(s.File));
            return await _apiClient.SendGallery(chatId, message, gallery);
        }
    }
}
