using Microsoft.Extensions.Hosting;
using Serilog;
using LogEventLevel = Serilog.Events;

namespace Logger
{
    public static class LoggerSetup
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var httpLoggerServer = context.Configuration.GetSection("Logging:HttpLogger:Server");
                var httpLevelLogger = context.Configuration.GetSection("Logging:HttpLogger:LogLevel");

                var httpSinkOption = new HttpSinkOption
                {
                    ServerAddress = httpLoggerServer?.Value ?? string.Empty
                };

                var isLogLevelParse = Enum.TryParse(httpLevelLogger?.Value, out LogEventLevel.LogEventLevel logLevel);


                configuration
                    .Enrich.FromLogContext()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.HttpServer(httpSinkOption, isLogLevelParse ? logLevel : LogEventLevel.LogEventLevel.Warning)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName ?? string.Empty)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName ?? string.Empty)
                    .ReadFrom.Configuration(context.Configuration);
            };
    }
}
