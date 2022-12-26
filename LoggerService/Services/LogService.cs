using Grpc.Core;
using LoggerService.Protos;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerBase
    {
        public override Task<WriteResponse> WriteLog(LogModel request, ServerCallContext context)
        {




            return Task.FromResult(new WriteResponse { Success = true });
        }
    }
}
