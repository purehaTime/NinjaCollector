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
        // TODO: Change defaults protobuf types to nullable types
        // https://stackoverflow.com/questions/4763875/does-protobuf-net-support-nullable-types
        public static GrpcHelper.DbService.Image ToGrpcData(this Image image, List<string> tags = null)
        {
            return new GrpcHelper.DbService.Image
            {
                Description = image.Description ?? string.Empty,
                Tags = { tags ?? image.Tags },
                File = image.File != null ? ByteString.CopyFrom(image.File) : ByteString.Empty,
                DirectLink = image.DirectLink ?? string.Empty,
                ImageType = image.ImageType ?? string.Empty,
                Height = image.Height,
                Width = image.Width,
                Name = image.Name ?? string.Empty
            };
        }
    }
}
