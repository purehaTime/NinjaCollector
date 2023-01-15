using DbService.Interfaces;
using DbService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class ImageService : IImageService
    {
        private readonly IGridFsRepository _gridFsRepository;
        private readonly IRepository<Image> _imageRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogger _logger;

        public ImageService(IGridFsRepository gridRepository, IRepository<Image> imageRepository, IHistoryService historyService, ILogger logger)
        {
            _gridFsRepository = gridRepository;
            _imageRepository = imageRepository;
            _historyService = historyService;
            _logger = logger;
        }

        public async Task<bool> SaveImage(MemoryStream imageStream, string description, List<string> tags, string directLink, int width, int height)
        {
            var saved = await InsertFile(imageStream);

            if (saved.Id == ObjectId.Empty)
            {
                return false;
            }

            var model = new Image
            {
                GridFsId = saved.Id,
                Name = saved.fileName,
                Tags = tags,
                Description = description,
                OriginalLink = directLink,
                Width = width,
                Height = height
            };

            return await InsertImage(model);
        }

        public async Task<bool> SaveImage(MemoryStream imageStream, Image image)
        {
            var saved = await InsertFile(imageStream);

            if (saved.Id == ObjectId.Empty)
            {
                return false;
            }

            image.GridFsId = saved.Id;
            image.Name = saved.fileName;

            return await InsertImage(image);
        }

        public async Task<(Image image, MemoryStream stream)> GetImageById(ObjectId id)
        {
            var filter = Builders<Image>.Filter.Eq("Id", id);
            var image = await _imageRepository.Find(filter, null!, CancellationToken.None);

            var stream = await _gridFsRepository.GetFileAsStream(image.GridFsId, null!, CancellationToken.None);
            return (image, stream);
        }

        public async Task<List<(Image image, MemoryStream stream)>> GetImagesForPost(ObjectId postId)
        {
            var filter = Builders<Image>.Filter.Eq(e => e.Id, postId);
            var images = await _imageRepository.FindMany(filter, null!, CancellationToken.None);

            var results = new List<(Image image, MemoryStream strem)>();

            foreach (var image in images)
            {
                var stream = await _gridFsRepository.GetFileAsStream(image.GridFsId, null!, CancellationToken.None);
                results.Add((image, stream));
            }

            return results;
        }

        public async Task<List<(Image image, MemoryStream stream)>> GetImagesByTags(List<string> tags, PosterSettings poster)
        {
            var filter = Builders<Image>.Filter.AnyIn(x => x.Tags, tags);

            var images = (await _imageRepository.FindMany(filter, null!, CancellationToken.None)).ToList();

            var historyIds = await _historyService.GetHistory(images.Select(s => s.Id), poster.Service, poster.ForGroup);

            var results = new List<(Image image, MemoryStream strem)>();
            var filteredImages = images.Where(w => historyIds.All(a => a != w.Id));

            foreach (var image in filteredImages)
            {
                var stream = await _gridFsRepository.GetFileAsStream(image.GridFsId, null!, CancellationToken.None);
                results.Add((image, stream));
            }

            return results;
        }

        private async Task<bool> InsertImage(Image model)
        {
            var result = await _imageRepository.Insert(model, null!, CancellationToken.None);

            if (!result)
            {
                _logger.Error("Cant save image to Image repository");
            }

            return result;
        }

        private async Task<(ObjectId Id, string fileName)> InsertFile(MemoryStream stream)
        {
            var fileName = new Guid().ToString();
            var fileId = await _gridFsRepository.AddFileAsStream(stream, fileName, null!, CancellationToken.None);

            if (fileId == null || fileId == ObjectId.Empty)
            {
                _logger.Error("Cant save image to gridFS");
                return (ObjectId.Empty, fileName);
            }

            return (fileId.Value, fileName);
        }
    }
}
