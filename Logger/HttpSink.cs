using GrpcHelper;
using Serilog.Core;
using Serilog.Events;

namespace Logger
{
    public class HttpSink : ILogEventSink
    {
        private HttpSinkOption _option;
        private LoggerServiceClient _loggerClient;

        public HttpSink(HttpSinkOption option)
        {
            _option = option;
            _loggerClient = new LoggerServiceClient(option.ServerAddress);
        }

        public void Emit(LogEvent logEvent)
        {
            if (_option.LogLevel > logEvent.Level) return;

            _loggerClient.WriteLog(logEvent.ToString())
                .GetAwaiter()
                .GetResult();
        }
    }
}
