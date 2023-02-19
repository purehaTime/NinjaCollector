using Newtonsoft.Json;

namespace RedditService.Model
{
    /// <summary>
    /// reddit types
    /// </summary>
    public class JsonContainer
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("e")]
        public string Type { get; set; }

        [JsonProperty("m")]
        public string Extension { get; set; }

        [JsonProperty("o")]
        public JsonImage[] Preview { get; set; }

        [JsonProperty("p")]
        public JsonImage[] Resized { get; set; }

        [JsonProperty("s")]
        public JsonImage Original { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class JsonImage
    {
        [JsonProperty("y")]
        public int Height { get; set; }

        [JsonProperty("x")]
        public int Width { get; set; }

        [JsonProperty("u")]
        public string Url { get; set; }
    }

    public class ImageDescription
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("url")]
        public string Link { get; set; }

        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public class PreviewImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
