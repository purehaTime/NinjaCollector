using Models.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class DbParserSettings : ParserSettings
    {
        [BsonId]
        public ObjectId DbId { get; set; }

    }
}
