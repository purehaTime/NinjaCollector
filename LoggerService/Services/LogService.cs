using Grpc.Core;
using GrpcHelper.LogService;
using LoggerService.Interfaces;
using LoggerService.Models;
using ILogger = Serilog.ILogger;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerBase
    {
        private IDatabase _db;
        private ILogger _systemLog;

        public LogService(IDatabase db, ILogger systemLog)
        {
            _db = db;
            _systemLog = systemLog;
        }

        public override async Task<WriteResponse> WriteLog(LogModel request, ServerCallContext context)
        {
            try
            {
                var result = await _db.Add(new Log
                {
                    Id = request.Id,
                    Message = request.Message,
                    Timestamp = request.Timestamp.ToDateTime(),
                    Application = request.Application
                });

                return new WriteResponse { Success = result };
            }
            catch (Exception err)
            {
                _systemLog.Fatal($"Error while write log: {err.Message}");
                return new WriteResponse { Success = false };
            }

        }

        public override Task<LogsResponse> GetLogs(LogsRequest request, ServerCallContext context)
        {
            return base.GetLogs(request, context);
        }
    }
}
