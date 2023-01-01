using GrpcHelper;
using GrpcHelper.Clients;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Logger
{
    public class HttpSink : ILogEventSink
    {
        private HttpSinkOption _option;
        private LoggerServiceClient _loggerClient;

        public HttpSink(HttpSinkOption option, IConfiguration config)
        {
            _option = option;
            var serviceConfig = new ServiceConfiguration(config);
            _loggerClient = new LoggerServiceClient(serviceConfig);
        }

        public void Emit(LogEvent logEvent)
        {
            if (_option.LogLevel > logEvent.Level) return;

            _loggerClient.WriteLog("logEvent", null, null)
                .GetAwaiter()
                .GetResult();
        }
    }
}
