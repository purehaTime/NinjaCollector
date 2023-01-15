using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class PosterSettings
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = null!;
        public string ForGroup { get; set; } = null!;
        public string Service { get; set; } = null!;

        /// <summary>
        /// in minutes
        /// </summary>
        public int TimeInterval { get; set; }

        /// <summary>
        /// enable this activate a plus\minus minutes for time interval
        /// </summary>
        public bool MixRandom { get; set; }

        public IEnumerable<string> Tags { get; set; } = null!;
    }
}
