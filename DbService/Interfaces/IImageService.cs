using DbService.Models;
using MongoDB.Bson;

namespace DbService.Interfaces
{
    public interface IImageService
    {
        public Task<ObjectId> SaveImage(MemoryStream image, string description, List<string> tags, string directLink,
            int width, int height);

        public Task<(Image image, MemoryStream stream)> GetImageById(ObjectId id);

        public Task<List<(Image image, MemoryStream stream)>> GetImagesByTags(List<string> tags);
    }
}
