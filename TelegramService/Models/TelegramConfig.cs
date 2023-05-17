namespace TelegramService.Models
{
    public class TelegramConfig
    {
        public string BotToken { get; set; }
        public List<long> AllowedUsersId { get; set; }
    }
}
