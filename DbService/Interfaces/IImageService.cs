using DbService.Models;
using MongoDB.Bson;

namespace DbService.Interfaces
{
    public interface IImageService
    {
        public Task<(bool Status, ObjectId SavedImage)> SaveImage(MemoryStream imageStream, GrpcHelper.DbService.Image image);

        public Task<(bool Status, ObjectId SavedImage)> SaveImage(byte[] imageBytes, GrpcHelper.DbService.Image image);

        public Task<(Image image, MemoryStream stream)> GetImageById(ObjectId id);

        public Task<List<(Image image, MemoryStream stream)>> GetImagesForPost(ObjectId postId);

        public Task<List<(Image image, MemoryStream stream)>> GetImagesByTags(List<string> tags, PosterSettings poster);
    }
}
