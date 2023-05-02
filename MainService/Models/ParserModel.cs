namespace MainService.Models
{
    public class ParserModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Group { get; set; }
        public int Timeout { get; set; }
        public int Hold { get; set; }
        public int Counts { get; set; }
        public int RetryAfterErrorCount { get; set; }
        public string Tags { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public string FromPostId { get; set; }
        public string UntilPostId { get; set; }
        public bool ByLastPostId { get; set; }
        public bool ContinueMonitoring { get; set; }
        public bool Disabled { get; set; }
        

        public bool IgnoreVideo { get; set; }
        public bool IgnoreRepost { get; set; }
        public string IgnoreWords { get; set; }
        public string IgnoreAuthors { get; set; }
        public string IgnoreTitles { get; set; }
        public string IgnoreDescriptions { get; set; }
    }
}
