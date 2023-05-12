using Google.Protobuf;
using ModelsHelper.Models;

namespace ModelsHelper.Mapping
{
    public static class ImageMapping
    {
        public static Image ToModel(this GrpcHelper.DbService.Image image)
        {
            return new Image
            {
                Description = image.Description,
                Tags = image.Tags.ToList(),
                File = image.File.ToByteArray(),
                DirectLink = image.DirectLink,
                ImageType = image.ImageType,
                Height = image.Height,
                Width = image.Width,
                Name = image.Name
            };
        }

        public static GrpcHelper.DbService.Image ToGrpcData(this Image image, List<string> tags = null)
        {
            return new GrpcHelper.DbService.Image
            {
                Description = image.Description,
                Tags = { tags ?? image.Tags },
                File = ByteString.CopyFrom(image.File),
                DirectLink = image.DirectLink,
                ImageType = image.ImageType,
                Height = image.Height,
                Width = image.Width,
                Name = image.Name
            };
        }
    }
}
