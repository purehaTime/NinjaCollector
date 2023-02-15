using Newtonsoft.Json.Linq;
using RedditService.Interfaces;
using RedditService.Model;

namespace RedditService.Services
{
    public class ParserGalleryService : IParserGalleryService
    {
        private readonly IFileDownloadService _fileDownload;

        public ParserGalleryService(IFileDownloadService fileDownload)
        {
            _fileDownload = fileDownload;
        }

        public async Task<List<JsonImage>> GetImageLinks(string urlToPost)
        {
            var links = new List<JsonImage>();
            var jsonLink = urlToPost.Replace("gallery", "comments") + ".json";
            var json = await _fileDownload.GetData(jsonLink);
            var myJObject = JArray.Parse(json);
            var result = myJObject.SelectTokens("[*].data.children.[*].data.media_metadata.*"); //parser path

            var images = result.Select(s => s.ToObject<JsonContainer>());
            //links.AddRange(images.Where(s => !string.IsNullOrEmpty(s.Url)));

            return links;
        }
    }
}
