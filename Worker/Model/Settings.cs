namespace Worker.Model
{
    public class Settings
    {
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
    }
}
