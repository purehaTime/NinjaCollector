using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class ParserSettings
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }

        /// <summary>
        /// in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Delay before first run
        /// </summary>
        public int Hold { get; set; }

        /// <summary>
        /// how many time worker should run
        /// </summary>
        public int Counts { get; set; }

        public int RetryAfterErrorCount { get; set; }

        public bool Disabled { get; set; }

        public List<string> Tags { get; set; }

        public string Source { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FromDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UntilDate { get; set; }

        public string FromPostId { get; set; }

        public string UntilPostId { get; set; }

        public bool ByLastPostId { get; set; }

        public bool ContinueMonitoring { get; set; }

        public bool IgnoreVideo { get; set; }
        public bool IgnoreRepost { get; set; }
        public List<string> IgnoreWords { get; set; }
        public List<string> IgnoreAuthors { get; set; }
        public List<string> IgnoreTitles { get; set; }
        public List<string> IgnoreDescriptions { get; set; }

    }
}
