namespace LoggerService.Models
{
    public class Log
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Application { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
