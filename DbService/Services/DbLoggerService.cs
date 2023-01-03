using Grpc.Core;
using GrpcHelper.DbService;
using MongoDB.Driver;

namespace DbService.Services
{
    public class DbLoggerService : DatabaseLog.DatabaseLogBase
    {
        public override Task<WriteLogResponse> WriteLog(DbLogModel request, ServerCallContext context)
        {
            var db = new MongoClient("mongodb://StoreDb");



            Console.WriteLine(request.Jsondata);
            return Task.FromResult(new WriteLogResponse { Success = true });
        }
    }
}
