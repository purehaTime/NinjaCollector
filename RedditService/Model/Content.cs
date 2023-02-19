namespace RedditService.Model
{
    public class Content
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string SubredditName { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public string OriginalLink { get; set; }
        public List<ImageContainer> Images { get; set; }
    }
}
