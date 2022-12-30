using Grpc.Core;
using GrpcHelper.DbService;

namespace DbService.Services
{
    public class DbLoggerService : DatabaseLog.DatabaseLogBase
    {
        public override Task<WriteLogResponse> WriteLog(DbLogModel request, ServerCallContext context)
        {
            Console.WriteLine(request.Jsondata);
            return Task.FromResult(new WriteLogResponse { Success = true });
        }
    }
}
