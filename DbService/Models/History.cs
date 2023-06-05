using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class History
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string EntityId { get; set; }

        public string Source { get; set; }

        public string Group { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }
    }
}
