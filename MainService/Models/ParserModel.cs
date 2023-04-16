namespace MainService.Models
{
    public class ParserModel
    {
        public string Description { get; set; }
        public string Group { get; set; }
        public string PostsCount { get; set; }
        public string LastLoadPostId { get; set; }
        public string StartFromLastLoadPost { get; set; }
        public string ByUpdate { get; set; }
        public string Timeout { get; set; }
        public string JobInterval { get; set; }
        public string Tags { get; set; }
        public string FromDate { get; set; }
        public string UntilDate { get; set; }
        public string Disabled { get; set; }
        public string ContinueMonitoring { get; set; }

        public string IgnoreVideo { get; set; }
        public string IgnoreRepost { get; set; }
        public string IgnoreWords { get; set; }
        public string IgnoreAuthors { get; set; }
        public string IgnoreTitles { get; set; }
        public string IgnoreDescriptions { get; set; }
    }
}
