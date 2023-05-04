using AutoFixture;
using FluentAssertions;
using Models.Models;
using Moq;
using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;
using Worker.Model;

namespace UnitTests.RedditServiceTests
{
    [TestFixture]
    public class RedditServiceTests : BaseTest
    {
        private Mock<IRedditApiClient> _apiClientMock;
        private Mock<IParserService> _parserServiceMock;

        private IRedditService _redditService;

        [OneTimeSetUp]
        public void Setup()
        {
            _apiClientMock = new Mock<IRedditApiClient>();
            _parserServiceMock = new Mock<IParserService>();

            _redditService = new global::RedditService.Services.RedditService(_apiClientMock.Object, _parserServiceMock.Object);
        }

        [Test]
        public void GetLastPost_ShouldReturn_Content()
        {
            var subName = Fixture.Create<string>();
            var post = Fixture.Create<Post>();
            var content = Fixture.Create<Content>();
            var filter = Fixture.Create<Filter>();

            _apiClientMock.Setup(s => s.GetLastPost(subName)).ReturnsAsync(post);
            _parserServiceMock.Setup(s => s.ParsePost(post, filter)).ReturnsAsync(content);

            var result = _redditService.GetLastPost(subName, filter)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Should().Be(content);
        }

        [Test]
        public void GetPostsBetweenDates_ShouldReturn_Contents()
        {
            var subName = Fixture.Create<string>();
            var posts = Fixture.CreateMany<Post>(5);
            var content = Fixture.Create<Content>();
            var filter = Fixture.Create<Filter>();
            var fromDate = DateTime.UtcNow;
            var toDate = DateTime.MinValue;

            _apiClientMock.Setup(s => s.GetPostsBetweenDates(subName, fromDate, toDate)).ReturnsAsync(posts);
            _parserServiceMock.Setup(s => s.ParsePost(It.IsAny<Post>(), filter)).ReturnsAsync(content);

            var result = _redditService.GetPostsBetweenDates(subName, fromDate, toDate, filter)
                .GetAwaiter()
                .GetResult()
                .ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().AllBeEquivalentTo(content);
        }

        [Test]
        public void GetPostsBetweenDates_ShouldReturn_Content()
        {
            var subName = Fixture.Create<string>();
            var posts = Fixture.CreateMany<Post>(5);
            var content = Fixture.Create<Content>();
            var filter = Fixture.Create<Filter>();
            var toDate = DateTime.MinValue;

            _apiClientMock.Setup(s => s.GetPostsBetweenDates(subName, It.IsAny<DateTime>(), toDate)).ReturnsAsync(posts);
            _parserServiceMock.Setup(s => s.ParsePost(It.IsAny<Post>(), filter)).ReturnsAsync(content);

            var result = _redditService.GetPostsToDate(subName, toDate, filter)
                .GetAwaiter()
                .GetResult()
                .ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().AllBeEquivalentTo(content);
        }

        [Test]
        public void GetPostsUntilPostId_ShouldReturn_Content()
        {
            var subName = Fixture.Create<string>();
            var posts = Fixture.CreateMany<Post>(5);
            var content = Fixture.Create<Content>();
            var untilPostId = Fixture.Create<string>();
            var filter = Fixture.Create<Filter>();

            _apiClientMock.Setup(s => s.GetPostsFromPostIdUntilPostId(subName, null, untilPostId)).ReturnsAsync(posts);
            _parserServiceMock.Setup(s => s.ParsePost(It.IsAny<Post>(), filter)).ReturnsAsync(content);

            var result = _redditService.GetPostsUntilPostId(subName, untilPostId, filter)
                .GetAwaiter()
                .GetResult()
                .ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().AllBeEquivalentTo(content);
        }
    }
}
