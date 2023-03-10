using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class Image
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId GridFsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string OriginalLink { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
