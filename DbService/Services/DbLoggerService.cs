using DbService.Models;
using Grpc.Core;
using GrpcHelper.DbService;
using MongoDB.Driver;

namespace DbService.Services
{
    public class DbLoggerService : DatabaseLog.DatabaseLogBase
    {
        public override async Task<WriteLogResponse> WriteLog(DbLogModel request, ServerCallContext context)
        {
            var mongoClient= new MongoClient("mongodb://StoreDb");

            var db = mongoClient.GetDatabase("logs");
            var coll = db.GetCollection<Log>("Log");

            var log = new Log
            {
                Timestamp = request.Timestamp.ToDateTime(),
                Json = request.Jsondata
            };


            await coll.InsertOneAsync(log);

            Console.WriteLine(request.Jsondata);
            return new WriteLogResponse { Success = true };
        }
    }
}
