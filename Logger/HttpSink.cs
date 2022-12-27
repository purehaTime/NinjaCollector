using Grpc.Net.Client;
using Serilog.Core;
using Serilog.Events;

namespace Logger
{
    public class HttpSink : ILogEventSink
    {
        private HttpSinkOption _option;
        //private GrpcChannel _channel;

        public HttpSink(HttpSinkOption option)
        {
            _option = option;
            
        }
        public void Emit(LogEvent logEvent)
        {
            using var channel = GrpcChannel.ForAddress(_option.ServerAddress);

            Console.WriteLine("test here !");
        }
    }
}
