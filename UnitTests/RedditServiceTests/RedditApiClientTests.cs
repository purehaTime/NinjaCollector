using AutoFixture;
using FluentAssertions;
using Moq;
using Reddit.Controllers;
using RedditService.API;
using RedditService.Interfaces;

namespace UnitTests.RedditService
{
    [TestFixture]
    public class RedditApiClientTests : BaseTest
    {
        private Mock<IRedditAsyncClient> _redditAsyncClient;

        private IRedditApiClient _apiClient;


        [OneTimeSetUp]
        public void Init()
        {
            _redditAsyncClient = new Mock<IRedditAsyncClient>();

            _apiClient = new RedditApiClient(_redditAsyncClient.Object);
        }

        [Test]
        public void GetLastPost_ShouldReturn_RedditPost()
        {
            var subName = Fixture.Create<string>();
            var subReddit = Fixture.Build<Subreddit>()
                .Without(w => w.Comments)
                .Create();
            var post = Fixture.Create<Post>();

            _redditAsyncClient.Setup(s => s.GetSubreddit(subName)).ReturnsAsync(subReddit);
            _redditAsyncClient.Setup(s => s.GetLastPost(subReddit)).ReturnsAsync(post);
            

           var result = _apiClient.GetLastPost(subName)
                .GetAwaiter()
                .GetResult();

           result.Should().NotBeNull();
           result.Should().Be(post);
        }

        [Test]
        public void GetPostsBetweenDates_ShouldReturn_RedditPosts()
        {
            var (subName, postId, untilPostId) = PrepareData();

            var result = _apiClient.GetPostsBetweenDates(subName, DateTime.MaxValue, DateTime.MinValue)
                .GetAwaiter()
                .GetResult()
                .ToList();

            Validate(result);
        }

        [Test]
        public void GetPostsFromDateUntilPostId_ShouldReturn_RedditPosts()
        {
            var (subName, postId, untilPostId) = PrepareData();

            var result = _apiClient.GetPostsFromDateUntilPostId(subName, DateTime.MinValue, untilPostId)
                .GetAwaiter()
                .GetResult()
                .ToList();

            Validate(result);
        }

        [Test]
        public void GetPostsFromPostIdUntilDate_ShouldReturn_RedditPosts()
        {
            var (subName, postId, untilPostId) = PrepareData();

            var result = _apiClient.GetPostsFromPostIdUntilDate(subName, postId, DateTime.MaxValue)
                .GetAwaiter()
                .GetResult()
                .ToList();

            Validate(result);
        }

        [Test]
        public void GetPostsFromPostIdUntilPostId_ShouldReturn_RedditPosts()
        {
            var (subName, postId, untilPostId) = PrepareData();

            var result = _apiClient.GetPostsFromPostIdUntilPostId(subName, postId, untilPostId)
                .GetAwaiter()
                .GetResult()
                .ToList();

            Validate(result);
        }

        private (string subName, string postId, string untilPostId) PrepareData()
        {
            var subName = Fixture.Create<string>();
            var subReddit = Fixture.Build<Subreddit>()
                .Without(w => w.Comments)
                .Create();

            var postId = Fixture.Create<string>();
            var untilPostId = Fixture.Create<string>();

            var posts = Fixture.Build<Post>()
                .With(w => w.Id, postId)
                .CreateMany(5)
                .ToList();

            _redditAsyncClient.Setup(s => s.GetSubreddit(subName)).ReturnsAsync(subReddit);
            _redditAsyncClient.Setup(s => s.Hold());
            _redditAsyncClient.SetupSequence(s => s.GetNewPosts(subReddit, It.IsAny<string>()))
                .ReturnsAsync(posts)
                .ReturnsAsync(new List<Post>());

            return (subName, postId, untilPostId);
        }

        private void Validate(List<Post> posts)
        {
            posts.Should().NotBeNull();
            posts.Should().HaveCount(5);
            posts.Should().BeEquivalentTo(posts);
        }
    }
}
