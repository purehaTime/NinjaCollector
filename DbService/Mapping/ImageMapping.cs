using DbService.Models;
using Google.Protobuf;
using MongoDB.Bson;

namespace DbService.Mapping
{
    public static class ImageMapping
    {
        public static Image ToDatabase(this GrpcHelper.DbService.Image image, ObjectId gridFsId, ObjectId? id = null)
        {
            return new Image
            {
                Id = id ?? ObjectId.GenerateNewId(),
                Description = image.Description,
                Tags = image.Tags.ToList(),
                DirectLink = image.DirectLink,
                ImageType = image.ImageType,
                Height = image.Height,
                Width = image.Width,
                Name = image.Name,
                GridFsId = gridFsId
            };
        }

        public static GrpcHelper.DbService.Image ToGrpcData(this Image image, byte[] file, List<string> tags = null)
        {
            return new GrpcHelper.DbService.Image
            {
                Description = image.Description,
                Tags = { tags ?? image.Tags },
                File = ByteString.CopyFrom(file),
                DirectLink = image.DirectLink,
                ImageType = image.ImageType,
                Height = image.Height,
                Width = image.Width,
                Name = image.Name
            };
        }
    }
}
