namespace RedditService.Model
{
    public class Image
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string DirectLink { get; set; }
        public string Description { get; set; }
        public string ImageType { get; set; }
        public byte[] Data { get; set; }
    }
}
