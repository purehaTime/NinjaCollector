using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Logger
{
    public static class LoggerSink
    {
        public static LoggerConfiguration HttpServer(this LoggerSinkConfiguration sinkConfiguration, string serverAddress, LogEventLevel level = LogEventLevel.Warning)
        {
            var sinkOpt = new HttpSinkOption
            {
                LogLevel = level,
                ServerAddress = serverAddress
            };

            return sinkConfiguration.Sink(new HttpSink(sinkOpt));
        }
    }
}
