using Serilog.Core;
using Serilog.Events;

namespace Logger
{
    public class HttpSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            Console.WriteLine("test here !");
        }
    }
}
