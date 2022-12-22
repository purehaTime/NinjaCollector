using Serilog.Core;
using Serilog.Events;

namespace Logger
{
    public class HttpSink : ILogEventSink
    {
        private HttpSinkOption _option;

        public HttpSink(HttpSinkOption option)
        {
            _option = option;
        }
        public void Emit(LogEvent logEvent)
        {
            
            Console.WriteLine("test here !");
        }
    }
}
