using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Logger
{
    public static class LoggerSink
    {
        public static LoggerConfiguration HttpServer(this LoggerSinkConfiguration sinkConfiguration, LogEventLevel level = LogEventLevel.Warning)
        {
            return sinkConfiguration.Sink<HttpSink>(level);
        }
    }
}
