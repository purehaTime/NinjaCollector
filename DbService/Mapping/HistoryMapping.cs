using DbService.Models;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using MongoDB.Bson;

namespace DbService.Mapping
{
    public static class HistoryMapping
    {
        public static History ToDatabase(this GrpcHelper.DbService.HistoryModel history, ObjectId? id = null)
        {
            return new History
            {
                Id = id ?? ObjectId.GenerateNewId(),
                Date = history.PostDate.ToDateTime(),
                EntityId = history.EntityId,
                Group = history.Group,
                Source = history.Source
            };
        }

        public static GrpcHelper.DbService.HistoryModel ToGrpcData(this History history, byte[] file, List<string> tags = null)
        {
            return new GrpcHelper.DbService.HistoryModel
            {
                PostDate = history.Date.ToTimestamp(),
                EntityId = history.EntityId,
                Group = history.Group,
                Source = history.Source
            };
        }
    }
}
