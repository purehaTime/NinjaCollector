using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DbService.Models
{
    public class PosterSettings
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

        /// <summary>
        /// Service name: tg, reddit, etc.
        /// </summary>
        public string Source { get; set; }
        public bool UseScheduling { get; set; }
        public int ScheduleInterval { get; set; }
        public bool UseRandom { get; set; }
        public bool IgnoreHistory { get; set; }
        public bool UseImagesOnly { get; set; }
        public bool UseSettingsText { get; set; }
        public string TextForPost { get; set; }
        public bool ContinuePosting { get; set; }
    }
}
