using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Logger
{
    public static class LoggerSink
    {
        public static LoggerConfiguration HttpServer(this LoggerSinkConfiguration sinkConfiguration, HttpSinkOption? option, LogEventLevel level = LogEventLevel.Warning)
        {
            var sinkOpt = option ?? new HttpSinkOption
            {
                ServerAddress = @"http:\\localhost:443"
            };
            return sinkConfiguration.Sink(new HttpSink(sinkOpt));
        }
    }
}
