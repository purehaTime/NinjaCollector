using DbService.Interfaces;
using DbService.Mapping;
using DbService.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class ImageService : IImageService
    {
        private readonly IGridFsService _gridFsService;
        private readonly IRepository<Image> _imageRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogger _logger;

        public ImageService(IGridFsService gridService, IRepository<Image> imageRepository, IHistoryService historyService, ILogger logger)
        {
            _gridFsService = gridService;
            _imageRepository = imageRepository;
            _historyService = historyService;
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

        public async Task<(Image image, MemoryStream stream)> GetImageById(ObjectId id)
        {
            var filter = Builders<Image>.Filter.Eq(e => e.Id, id);
            var image = await _imageRepository.Find(filter, null, CancellationToken.None);

            var stream = await _gridFsService.GetFileAsStream(image.GridFsId, null!, CancellationToken.None);
            return (image, stream);
        }

        public async Task<List<(Image image, MemoryStream stream)>> GetImagesForPost(ObjectId postId)
        {
            var filter = Builders<Image>.Filter.Eq(e => e.Id, postId);
            var images = await _imageRepository.FindMany(filter, null, CancellationToken.None);

            var results = await GetImagesFromGridFs(images);
            return results;
        }

        public async Task<List<(Image image, MemoryStream stream)>> GetImagesByTags(List<string> tags,
            PosterSettings poster)
        {
            var filter = Builders<Image>.Filter.AnyIn(x => x.Tags, tags);

            var images = (await _imageRepository.FindMany(filter, null, CancellationToken.None)).ToList();

            var filteredImages = images;
            if (!poster.IgnoreHistory)
            {
                var histories = await _historyService.GetHistory(images.Select(s => s.Id), poster.Source, poster.Group);
                filteredImages = images.Where(w => histories.All(a => a.EntityId != w.Id)).ToList();
            }

            var results = await GetImagesFromGridFs(filteredImages);
            return results;
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
