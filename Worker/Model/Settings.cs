namespace Worker.Model
{
    public class Settings
    {
        public string Id { get; set; }
        public string ApiName { get; set; }
        public string ForGroup { get; set; }

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

        public List<string> TagsForPosts { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? UntilDate { get; set; }

        public bool UpdateFromDate { get; set; }

        public string FromPostId { get; set; }

        public string UntilPostId { get; set; }

        public bool ByLastPostId { get; set; }

        public bool ContinueMonitoring { get; set; }

        public bool Disabled { get; set; }
    }
}
