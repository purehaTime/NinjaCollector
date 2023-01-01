using Grpc.Core;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerBase
    {
        private IDatabaseServiceClient _dbClient;

        public LogService(IDatabaseServiceClient dbClient)
        {
            _dbClient = dbClient;
        }

        public override async Task<WriteResponse> WriteLog(LogModel request, ServerCallContext context)
        {
            _dbClient.WriteLogToDb(new DbLogModel
            {
                Jsondata = "",
            });
            return await Task.FromResult(new WriteResponse() { Success = true });
        }

        public override Task<LogsResponse> GetLogs(LogsRequest request, ServerCallContext context)
        {
            return base.GetLogs(request, context);
        }
    }
}
