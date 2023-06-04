using DbService.Interfaces;
using DbService.Mapping;
using DbService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class ImageService : IImageService
    {
        private readonly IGridFsService _gridFsService;
        private readonly IRepository<Image> _imageRepository;
        private readonly IHistoryService _historyService;
        private readonly ISettingsService _settingsService;
        private readonly ILogger _logger;

        public ImageService(IGridFsService gridService, IRepository<Image> imageRepository, IHistoryService historyService, ISettingsService settingsService, ILogger logger)
        {
            _gridFsService = gridService;
            _imageRepository = imageRepository;
            _historyService = historyService;
            _settingsService = settingsService;
            _logger = logger;
        }

        public async Task<(bool, ObjectId)> SaveImage(MemoryStream imageStream, GrpcHelper.DbService.Image image)
        {
            var saved = await InsertFile(imageStream);
            var result = await SaveImage(saved.Id, saved.fileName, image);
            return result;
        }

        public async Task<(bool, ObjectId)> SaveImage(byte[] imageBytes, GrpcHelper.DbService.Image image)
        {
            var saved = await InsertFile(imageBytes);
            var result = await SaveImage(saved.Id, saved.fileName, image);
            return result;
        }

        public async Task<(bool Status, ObjectId SavedImage)> SaveImage(GrpcHelper.DbService.Image image)
        {
            var bytes = image.File.ToByteArray();
            var saved = await InsertFile(bytes);
            var result = await SaveImage(saved.Id, saved.fileName, image);
            return result;
        }

        public async Task<(Image image, MemoryStream stream)> GetImageById(ObjectId id)
        {
            var filter = Builders<Image>.Filter.Eq(e => e.Id, id);
            var image = await _imageRepository.Find(filter, null, CancellationToken.None);

            var stream = await _gridFsService.GetFileAsStream(image.GridFsId, null!, CancellationToken.None);
            return (image, stream);
        }

        public async Task<List<(Image image, MemoryStream stream)>> GetImagesBySettingId(string settingId)
        {
            var setting = await _settingsService.GetPosterSetting(settingId);
            if (setting == null)
            {
                return null;
            }

            var filter = Builders<Image>.Filter.AnyIn(x => x.Tags, setting.Tags);
            var images = (await _imageRepository.FindMany(filter, null, CancellationToken.None)).ToList();

            if (!setting.IgnoreHistory)
            {
                var histories = await _historyService.GetHistory(images.Select(s => s.DirectLink), setting.Source, setting.Group);
                images = images.ExceptBy(histories.Select(s => s.EntityId), post => post.DirectLink).ToList();
                if (images.Count == 0)
                {
                    _logger.Information("All posts was posted by history");
                    return null;
                }
            }

            var results = await GetImagesFromGridFs(images);
            return results;
        }

        public async Task<(Image image, MemoryStream steam)> GetImageBySettingId(string settingId)
        {
            var setting = await _settingsService.GetPosterSetting(settingId);
            if (setting == null)
            {
                return (null, null);
            }

            var filter = Builders<Image>.Filter.AnyIn(x => x.Tags, setting.Tags);
            var images = (await _imageRepository.FindMany(filter, null, CancellationToken.None)).ToList();

            if (!setting.IgnoreHistory)
            {
                var histories = await _historyService.GetHistory(images.Select(s => s.DirectLink), setting.Source, setting.Group);
                images = images.ExceptBy(histories.Select(s => s.EntityId), post => post.DirectLink).ToList();
                if (images.Count > 0)
                {
                    _logger.Information("All posts was posted by history");
                    return (null, null);
                }
            }

            var rnd = new Random();
            var image = setting.UseRandom ? images[rnd.Next(images.Count - 1)] : images.First();

            var results = await GetImagesFromGridFs(new List<Image> { image });
            return results.FirstOrDefault();
        }

        private async Task<(bool, ObjectId)> SaveImage(ObjectId id, string fileName, GrpcHelper.DbService.Image image)
        {
            if (id == ObjectId.Empty)
            {
                return (false, ObjectId.Empty);
            }

            var model = image.ToDatabase(id);
            model.Name = fileName;

            var result = await InsertImage(model);

            return (result, id);
        }

        private async Task<bool> InsertImage(Image model)
        {
            var result = await _imageRepository.Insert(model, null, CancellationToken.None);

            if (!result)
            {
                _logger.Error("Cant save image to Image repository");
            }

            return result;
        }

        private async Task<(ObjectId Id, string fileName)> InsertFile(MemoryStream stream)
        {
            var fileName = new Guid().ToString();
            var fileId = await _gridFsService.AddFileAsStream(stream, fileName, null, CancellationToken.None);

            var result = InsertFileValidation(fileId, fileName);
            return result;
        }

        private async Task<(ObjectId Id, string fileName)> InsertFile(byte[] bytes)
        {
            var fileName = new Guid().ToString();
            var fileId = await _gridFsService.AddFileAsBytes(bytes, fileName, null, CancellationToken.None);

            var result = InsertFileValidation(fileId, fileName);
            return result;
        }

        private (ObjectId Id, string fileName) InsertFileValidation(ObjectId? id, string fileName)
        {
            if (id == null || id == ObjectId.Empty)
            {
                _logger.Error("Cant save image to gridFS");
                return (ObjectId.Empty, fileName);
            }

            return (id.Value, fileName);
        }

        private async Task<List<(Image image, MemoryStream strem)>> GetImagesFromGridFs(IEnumerable<Image> images)
        {
            var results = new List<(Image image, MemoryStream strem)>();
            foreach (var image in images)
            {
                var stream = await _gridFsService.GetFileAsStream(image.GridFsId, null, CancellationToken.None);
                results.Add((image, stream));
            }

            return results;
        }
    }
}
