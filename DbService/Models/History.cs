using MongoDB.Bson;

namespace DbService.Models
{
    public class History
    {
        public ObjectId Id { get; set; }

        public ObjectId EntityId { get; set; }

        public string Service { get; set; } = null!;

        public string ForGroup { get; set; } = null!;

        public DateTime Date { get; set; }
    }
}
