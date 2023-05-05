namespace Models.DataModels
{
    public class Filter
    {
        public bool IgnoreVideo { get; set; }
        public bool IgnoreRepost { get; set; }
        public List<string> IgnoreWords { get; set; }
        public List<string> IgnoreAuthors { get; set; }
        public List<string> IgnoreTitles { get; set; }
        public List<string> IgnoreDescriptions { get; set; }
    }
}
