using Newtonsoft.Json;

namespace RedditService.Model
{
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
        public long Height { get; set; }

        [JsonProperty("x")]
        public long Width { get; set; }

        [JsonProperty("u")]
        public string Url { get; set; }
    }
}
