using DbService.Models;
using MongoDB.Bson;

namespace DbService.Interfaces
{
    public interface IImageService
    {
        public Task<(bool Status, ObjectId SavedImage)> SaveImage(MemoryStream image, string description, List<string> tags, string directLink,
            int width, int height);

        public Task<(bool Status, ObjectId SavedImage)> SaveImage(byte[] imageBytes, string description, List<string> tags,
            string directLink, int width, int height);

        public Task<(Image image, MemoryStream stream)> GetImageById(ObjectId id);

        public Task<List<(Image image, MemoryStream stream)>> GetImagesForPost(ObjectId postId);

        public Task<List<(Image image, MemoryStream stream)>> GetImagesByTags(List<string> tags, PosterSettings poster);
    }
}
