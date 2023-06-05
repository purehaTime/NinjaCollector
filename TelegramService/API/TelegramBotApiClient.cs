using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.Interfaces;
using ILogger = Serilog.ILogger;
using InputFile = Telegram.Bot.Types.InputFile;

namespace TelegramService.API
{
    public class TelegramBotApiClient : ITelegramBotApiClient
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        public TelegramBotApiClient(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task<bool> SendMessage(string chatId, string message)
        {
            try
            {
                var chat = new ChatId(chatId);
                await _botClient.SendTextMessageAsync(chat, message);
                return true;
            }
            catch (Exception err)
            {
                _logger.Error(err, "Can't SendMessage to telegram");
                return false;
            }
        }

        public async Task<bool> SendPicture(string chatId, string message, MemoryStream picture)
        {
            try
            {
                var chat = new ChatId(chatId);
                var stream = InputFile.FromStream(picture);
                await _botClient.SendPhotoAsync(chat, stream, caption: message);
                return true;
            }
            catch (Exception err)
            {
                _logger.Error(err, "Can't SendPicture to telegram");
                return false;
            }
        }

        public async Task<bool> SendGallery(string chatId, string message, IEnumerable<MemoryStream> pictures)
        {
            try
            {
                var chat = new ChatId(chatId);
                var media = new List<IAlbumInputMedia>();
                foreach (var image in pictures)
                {
                    media.Add(new InputMediaPhoto(InputFile.FromStream(image, Guid.NewGuid().ToString())));
                }

                await _botClient.SendMediaGroupAsync(chat, media);
                return true;
            }
            catch (Exception err)
            {
                _logger.Error(err, "Can't SendGallery to telegram");
                return false;
            }
        }
    }
}
