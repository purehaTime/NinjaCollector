using Grpc.Core;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerBase
    {


        public LogService(Logger.LoggerClient test, ILoggerServiceClient client)
        {
            Console.WriteLine(client);
            Console.WriteLine(test);
        }

        public override async Task<WriteResponse> WriteLog(LogModel request, ServerCallContext context)
        {
            
            return await Task.FromResult(new WriteResponse() { Success = true });
        }

        public override Task<LogsResponse> GetLogs(LogsRequest request, ServerCallContext context)
        {
            return base.GetLogs(request, context);
        }
    }
}
