using ModelsHelper.Models;

namespace TelegramService.Interfaces
{
    public interface ITelegramBotService
    {
        public Task<bool> SendPost(string chatId, string message, List<Image> images);
    }
}
