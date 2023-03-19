using Newtonsoft.Json.Linq;
using RedditService.Interfaces;
using RedditService.Model;
using ILogger = Serilog.ILogger;

namespace RedditService.Services
{
    public class ParserGalleryService : IParserGalleryService
    {
        private readonly IFileDownloadService _fileDownload;
        private readonly ILogger _logger;

        public ParserGalleryService(IFileDownloadService fileDownload, ILogger logger)
        {
            _fileDownload = fileDownload;
            _logger = logger;
        }

        public async Task<IEnumerable<Image>> GetImageLinks(string urlToGallery)
        {
            try
            {
                var jsonLink = urlToGallery.Replace("gallery", "comments") + ".json";
                var json = await _fileDownload.GetData(jsonLink);
                var jsonObject = JArray.Parse(json);
                var result = jsonObject.SelectTokens("[*].data.children.[*].data.media_metadata.*"); //parser path
                var imagesJson = result.Select(s => s.ToObject<JsonContainer>());

                var imagesData = jsonObject.SelectTokens("[*].data.children.[*].data.gallery_data.*");
                var imagesDescription = imagesData.SelectMany(s => s.ToObject<ImageDescription[]>());

                var images = imagesJson.Select(s => new Image
                {
                    DirectLink = s.Original?.Url,
                    ImageType = s.Extension,
                    Height = s.Original?.Height ?? 0,
                    Width = s.Original?.Width ?? 0,
                    Name = s.Id,
                    Description = imagesDescription.FirstOrDefault(f => f.MediaId == s.Id)?.Caption
                });

                return images;
            }
            catch (Exception err)
            {
                _logger.Error($"Can't parse url {urlToGallery} in reddit service", err);
            }

            return null;
        }
    }
}
