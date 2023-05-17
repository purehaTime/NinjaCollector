namespace TelegramService.Interfaces
{
    public interface ITelegramBotApiClient
    {
        public Task<bool> SendMessage(string message);
        public Task<bool> SendPicture(string message, MemoryStream picture);
    }
}
