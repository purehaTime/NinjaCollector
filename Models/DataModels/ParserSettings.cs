namespace Models.DataModels
{
    public class ParserSettings
    {
        public ParserSettings()
        {
            Filter = new Filter();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
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

        public List<string> Tags { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime UntilDate { get; set; }

        public string FromPostId { get; set; }

        public string UntilPostId { get; set; }

        public bool ByLastPostId { get; set; }

        public bool ContinueMonitoring { get; set; }

        public bool Disabled { get; set; }

        //Filters
        public Filter Filter { get; set; }

    }
}
