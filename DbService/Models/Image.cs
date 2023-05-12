using ModelsHelper.Models;
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
        public int Width { get; set; }
        public int Height { get; set; }
        public string DirectLink { get; set; }
        public string Description { get; set; }
        public string ImageType { get; set; }
        public List<string> Tags { get; set; }
    }
}
