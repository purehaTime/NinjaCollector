using Grpc.Core;
using GrpcHelper.LogService;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerBase
    {
        public override async Task<WriteResponse> WriteLog(LogModel request, ServerCallContext context)
        {
            Console.WriteLine("here");
            return await Task.FromResult(new WriteResponse() { Success = true });
        }
    }
}
