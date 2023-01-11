using Grpc.Net.Client;
using GrpcHelper.Clients;
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
            var channel = GrpcChannel.ForAddress(_option.ServerAddress);
            var client = new GrpcHelper.LogService.Logger.LoggerClient(channel);
            _loggerClient = new LoggerServiceClient(client);
        }

        public void Emit(LogEvent logEvent)
        {
            if (_option.LogLevel > logEvent.Level) return;

            var eventId = logEvent.Properties["ActionId"].ToString();
            var appName = logEvent.Properties["Application"].ToString();
            var message = logEvent.MessageTemplate.Text + "\r\n" + (logEvent.Exception?.Message ?? "");

            _loggerClient.WriteLog(message, eventId, appName)
                .GetAwaiter()
                .GetResult();
        }
    }
}
