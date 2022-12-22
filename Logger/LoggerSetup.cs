using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Logger
{
    public static class LoggerSetup
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var httpLogLevel = context.Configuration.GetSection("HttpLogLevel");
                
                configuration
                    .Enrich.FromLogContext()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.HttpServer()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName ?? string.Empty)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName ?? string.Empty)
                    .ReadFrom.Configuration(context.Configuration);
            };
    }
}
