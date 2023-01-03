using Grpc.Core;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerBase
    {
        private IDatabaseServiceClient _dbClient;
        private ILogger _systemLog;

        public LogService(IDatabaseServiceClient dbClient, ILogger systemLog)
        {
            _dbClient = dbClient;
            _systemLog = systemLog;
        }

        public override async Task<WriteResponse> WriteLog(LogModel request, ServerCallContext context)
        {
            try
            {
                var result = await _dbClient.WriteLogToDb(new DbLogModel
                {
                    Jsondata = JsonConvert.SerializeObject(request),
                    Timestamp = request.Timestamp
                });
                return new WriteResponse { Success = result };
            }
            catch (Exception err)
            {
                _systemLog.Fatal($"Database logger has exception: {err.Message}");
                return new WriteResponse { Success = false };
            }
        }

        public override Task<LogsResponse> GetLogs(LogsRequest request, ServerCallContext context)
        {
            return base.GetLogs(request, context);
        }
    }
}
