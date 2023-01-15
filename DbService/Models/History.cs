using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class History
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId EntityId { get; set; }

        public string Service { get; set; } = null!;

        public string ForGroup { get; set; } = null!;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }
    }
}
