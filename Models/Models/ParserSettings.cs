namespace ModelsHelper.Models
{
    public class ParserSettings : Settings
    {
        public ParserSettings()
        {
            Filter = new Filter();
        }

        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public string FromPostId { get; set; }
        public string UntilPostId { get; set; }
        public bool ByLastPostId { get; set; }
        public bool ContinueMonitoring { get; set; }

        //Filters
        public Filter Filter { get; set; }

    }
}
