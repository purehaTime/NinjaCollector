namespace LoggerService.Models
{
    public class Log
    {
        public string Id { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Application { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
