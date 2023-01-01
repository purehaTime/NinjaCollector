using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Logger
{
    public static class LoggerSink
    {
        public static LoggerConfiguration HttpServer(this LoggerSinkConfiguration sinkConfiguration, IConfiguration config, LogEventLevel level = LogEventLevel.Warning)
        {
            var sinkOpt = new HttpSinkOption
            {
                LogLevel = level
            };

            return sinkConfiguration.Sink(new HttpSink(sinkOpt, config));
        }
    }
}
