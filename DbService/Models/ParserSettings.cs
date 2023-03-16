using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class ParserSettings
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Group { get; set; }

        /// <summary>
        /// How much posts will be upload (more 0 - until end of source)
        /// this work only if By** options disabled
        /// </summary>
        public int PostsCount { get; set; }

        public string LastLoadPostId { get; set; }

        /// <summary>
        /// upload posts from last post id and previous posts by PostCount
        /// </summary>
        public bool StartFromLastLoadPost { get; set; }

        /// <summary>
        /// upload posts from LastPostId or Interval until last post in source
        /// </summary>
        public bool ByUpdate { get; set; }

        /// <summary>
        /// Load posts from (or LastLoadPostId if StartFromLastLoadPost is true) now until time post (in hours)
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// how often run job (in seconds)
        /// </summary>
        public int JobInterval { get; set; }

        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Hold in seconds before first run
        /// </summary>
        public int Hold { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public bool Disabled { get; set; }
        public bool ContinueMonitoring { get; set; }
    }
}
