namespace ModelsHelper.Models
{
    public class Post
    {
        public Post()
        {
            Tags = new List<string>();
            Images = new List<Image>();
        }

        public string PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public List<Image> Images { get; set; }
        public List<string> Tags { get; set; }
        public string Source { get; set; }
        public string OriginalLink { get; set; }
        public DateTime PostDate { get; set; }
    }
}
