using TelegramService.Models;

namespace TelegramService.Interfaces
{
    public interface ITelegramBotApiClient
    {
        public Task<bool> SendMessage(string chatId, string message);
        public Task<bool> SendPicture(string chatId, string message, MemoryStream picture);
        public Task<bool> SendGallery(string chatId, string message, IEnumerable<ImageGallery> pictures);

    }
}
