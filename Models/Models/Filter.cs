namespace ModelsHelper.Models
{
    public class Filter
    {
        public Filter()
        {
            IgnoreWords = new List<string>();
            IgnoreAuthors = new List<string>();
            IgnoreTitles = new List<string>();
            IgnoreDescriptions = new List<string>();
        }

        public bool IgnoreVideo { get; set; }
        public bool IgnoreRepost { get; set; }
        public List<string> IgnoreWords { get; set; }
        public List<string> IgnoreAuthors { get; set; }
        public List<string> IgnoreTitles { get; set; }
        public List<string> IgnoreDescriptions { get; set; }
    }
}
