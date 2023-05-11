using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static GrpcHelper.DbService.Image ToGrpcData(this Image image)
        {
            return new GrpcHelper.DbService.Image
            {
                Description = image.Description,
                Tags = { image.Tags },
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
