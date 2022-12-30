using Serilog.Events;

namespace Logger
{
    public class HttpSinkOption
    {
        public string ServerAddress { get; set; } = null!;
        public LogEventLevel LogLevel { get; set; }
    }
}
