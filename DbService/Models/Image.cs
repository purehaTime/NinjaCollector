using MongoDB.Bson;

namespace DbService.Models
{
    public class Image
    {
        public ObjectId Id { get; set; }
        public ObjectId GridFsId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
        public string OriginalLink { get; set; } = null!;
        public IEnumerable<string> Tags { get; set; } = null!;
    }
}
