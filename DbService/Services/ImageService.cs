﻿using DbService.Interfaces;
using DbService.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task<ObjectId> SaveImage(MemoryStream image, string description, List<string> tags, string directLink, int width, int height)
        {
            var fileName = new Guid().ToString();
            var fileId = await _gridFsRepository.AddFileAsStream(image, fileName, null!, CancellationToken.None);

            if (fileId == null)
            {
                _logger.Error("Cant save image to gridFS");
                return ObjectId.Empty;
            }

            var model = new Image
            {
                GridFsId = fileId.Value,
                Name = fileName,
                Tags = tags,
                Description = description,
                OriginalLink = directLink,
                Width = width,
                Height = height
            };

            var result = await _imageRepository.Insert(model, null!, CancellationToken.None);

            if (!result)
            {
                _logger.Error("Cant save image to Image repository");
            }

            return fileId.Value;
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
    }
}