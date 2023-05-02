namespace MainService.Models
{
    public class ParserModel
    {
        public string Description { get; set; }
        public string Group { get; set; }
        public int PostsCount { get; set; }
        public string StartFromLastLoadPost { get; set; }
        public string ByUpdate { get; set; }
        public int Timeout { get; set; }
        public int JobInterval { get; set; }
        public int Hold { get; set; }
        public string Tags { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public bool Disabled { get; set; }
        public bool ContinueMonitoring { get; set; }

        public bool IgnoreVideo { get; set; }
        public bool IgnoreRepost { get; set; }
        public string IgnoreWords { get; set; }
        public string IgnoreAuthors { get; set; }
        public string IgnoreTitles { get; set; }
        public string IgnoreDescriptions { get; set; }
    }
}
