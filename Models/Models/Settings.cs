namespace ModelsHelper.Models
{
    public abstract class Settings
    {
        public string Id { get; set; }
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
    }
}
