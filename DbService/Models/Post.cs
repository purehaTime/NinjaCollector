using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; } = null!;
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Source { get; set; }
        public string OriginalLink { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime PostDate { get; set; }
    }
}
