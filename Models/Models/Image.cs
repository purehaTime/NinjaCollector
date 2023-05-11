namespace ModelsHelper.Models
{
    public class Image
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string DirectLink { get; set; }
        public string Description { get; set; }
        public string ImageType { get; set; }
        public List<string> Tags { get; set; }
        public byte[] File { get; set; }
    }
}
