using Models.Models;
using RedditService.Interfaces;
using Worker.Model;
using Post = Reddit.Controllers.Post;

namespace RedditService.Services
{
    public class FilterService : IFilterService
    {
        public bool IsValid(Post post, Filter filter)
        {
            var valid = true;

            if (filter.IgnoreVideo)
            {
                valid &= !IsVideo(post.Listing.Domain);
            }

            if (filter.IgnoreRepost)
            {
                valid &= !IsRepost(post);
            }

            if (filter.IgnoreWords?.Count > 0)
            {
                valid &= !IsIgnoreByWord(post.Listing.SelfText, filter.IgnoreWords.ToList());
            }

            if (filter.IgnoreTitles?.Count > 0)
            {
                valid &= !IsIgnoreByWord(post.Listing.Title, filter.IgnoreTitles.ToList());
            }

            if (filter.IgnoreAuthors?.Count > 0)
            {
                valid &= !IsIgnoreByWord(post.Listing.Author, filter.IgnoreAuthors.ToList());
            }


            return valid;
        }

        private static bool IsVideo(string link)
        {
            return link == "v.redd.it";
        }

        private static bool IsRepost(Post post)
        {
            return false;
        }

        private static bool IsIgnoreByWord(string text, IEnumerable<string> ignoredWords)
        {
            return ignoredWords.Any(a => a == text);
        }
    }
}
