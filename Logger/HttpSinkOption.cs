using Serilog.Events;

namespace Logger
{
    public class HttpSinkOption
    {
        public LogEventLevel LogLevel { get; set; }
        public string ServerAddress { get; set; }
    }
}
