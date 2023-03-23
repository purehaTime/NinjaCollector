namespace RedditService.Model
{
    public class RedditConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string AppSecret { get; set; }
        public int AntiSpamTimeout { get; set; }
    }
}
