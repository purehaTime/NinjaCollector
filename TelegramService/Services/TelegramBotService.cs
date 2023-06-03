using ModelsHelper.Mapping;
using ModelsHelper.Models;
using TelegramService.Interfaces;
using TelegramService.Models;

namespace TelegramService.Services
{
    public class TelegramBotService  : ITelegramBotService
    {
        private readonly ITelegramBotApiClient _apiClient;

        public TelegramBotService(ITelegramBotApiClient apiClient)
        {
            _apiClient = apiClient;
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

            var gallery = images.Select(s => new ImageGallery
            {
                Name = string.IsNullOrEmpty(s.Name) ? Guid.NewGuid().ToString() : s.Name,
                Image = new MemoryStream(s.File)
            });

            return await _apiClient.SendGallery(chatId, message, gallery);
        }
    }
}
