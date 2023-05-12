using DbService.Models;
using Google.Protobuf.WellKnownTypes;

namespace DbService.Mapping
{
    public static class PostMapping
    {
        public static Post ToDatabase(this GrpcHelper.DbService.Post post, List<Image> images)
        {
            return new Post
            {
                PostId = post.PostId,
                Description = post.Description,
                Group = post.Group,
                Source = post.Source,
                OriginalLink = post.OriginalLink,
                PostDate = post.PostDate.ToDateTime(),
                Text = post.Text,
                Title = post.Title,
                UserName = post.UserName,
                Tags = post.Tags.ToList(),
                Images = images
            };
        }

        public static GrpcHelper.DbService.Post ToGrpcData(this Post post, List<GrpcHelper.DbService.Image> images, List<string> tags = null)
        {
            return new GrpcHelper.DbService.Post
            {
                PostId = post.PostId,
                Description = post.Description,
                Group = post.Group,
                Source = post.Source,
                OriginalLink = post.OriginalLink,
                PostDate = Timestamp.FromDateTime(post.PostDate),
                Text = post.Text,
                Title = post.Title,
                UserName = post.UserName,
                Tags = { tags ?? post.Tags },
                Images = { images }
            };
        }
    }
}
