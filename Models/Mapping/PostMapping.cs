using Google.Protobuf.WellKnownTypes;
using ModelsHelper.Models;

namespace ModelsHelper.Mapping
{
    public static class PostMapping
    {
        public static Post ToModel(this GrpcHelper.DbService.Post post)
        {
            return new Post
            {
                Id = post.PostId,
                Description = post.Description,
                Group = post.Group,
                Source = post.Source,
                OriginalLink = post.OriginalLink,
                PostDate = post.PostDate.ToDateTime(),
                Text = post.Text,
                Title = post.Title,
                UserName = post.UserName,
                Tags = post.Tags.ToList(),
                Images = post.Images.Select(s => s.ToModel()).ToList()
            };
        }

        public static GrpcHelper.DbService.Post ToGrpcData(this Post post, List<string> tags = null)
        {
            return new GrpcHelper.DbService.Post
            {
                PostId = post.Id,
                Description = post.Description,
                Group = post.Group,
                Source = post.Source,
                OriginalLink = post.OriginalLink,
                PostDate = Timestamp.FromDateTime(post.PostDate),
                Text = post.Text,
                Title = post.Title,
                UserName = post.UserName,
                Tags = { tags ?? post.Tags },
                Images = { post.Images.Select(s => s.ToGrpcData(tags)) }
            };
        }
    }
}
