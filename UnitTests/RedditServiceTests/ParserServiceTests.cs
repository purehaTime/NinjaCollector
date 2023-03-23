using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;
using RedditService.Services;
using RestSharp;

namespace UnitTests.RedditService
{
    [TestFixture]
    public class ParserServiceTests : BaseTest
    {
        private Mock<IParserGalleryService> _parserGalleryServiceMock;
        private Mock<IFileDownloadService> _fileDownloadServiceMock;

        private IParserService _parserService;


        [OneTimeSetUp]
        public void Init()
        {
            _parserGalleryServiceMock = new Mock<IParserGalleryService>();
            _fileDownloadServiceMock = new Mock<IFileDownloadService>();

            _parserService = new ParserService(_parserGalleryServiceMock.Object, _fileDownloadServiceMock.Object);
        }

        [Test]
        public void ParsePost_ShouldReturn_Null()
        {
            var link = "v.redd.it";
            var post = Fixture.Create<Post>();
            post.Listing.Domain = link;

            var result = _parserService.ParsePost(post)
                .GetAwaiter()
                .GetResult();

            result.Should().BeNull();
        }

        [Test]
        public void ParsePost_ShouldReturn_Contents_WithGallery()
        {
            var link = "reddit.com/gallery";
            var post = Fixture.Create<Post>();
            post.Listing.URL = link;

            var images = Fixture.CreateMany<Image>(5);

            var bytes = Fixture.Create<byte[]>();

            _parserGalleryServiceMock.Setup(s => s.GetImageLinks(link)).ReturnsAsync(images);
            _fileDownloadServiceMock.Setup(s => s.GetFile(It.IsAny<string>())).ReturnsAsync(bytes);

            var result = _parserService.ParsePost(post)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Images.Should().HaveCount(5);
            result.Description.Should().Be(post.Listing.LinkFlairText);
            result.Text.Should().Be(post.Listing.SelfText);
            result.Id.Should().Be(post.Id);
            result.UserName.Should().Be(post.Author);
            result.Title.Should().Be(post.Title);
        }

        [Test]
        public void ParsePost_ShouldReturn_Contents_WithSingleImage()
        {
            var link = "test.com";
            var bytes = Fixture.Create<byte[]>();
            var post = Fixture.Create<Post>();
            post.Listing.URL = link;
            post.Listing.Preview = JObject.Parse("{\"images\":[{\"source\":{\"url\":\"https://test.com\",\"width\":32,\"height\":64}}]}");

            _fileDownloadServiceMock.Setup(s => s.GetFile(It.IsAny<string>())).ReturnsAsync(bytes);

            var result = _parserService.ParsePost(post)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Images.Should().HaveCount(1);
            result.Description.Should().Be(post.Listing.LinkFlairText);
            result.Text.Should().Be(post.Listing.SelfText);
            result.Id.Should().Be(post.Id);
            result.UserName.Should().Be(post.Author);
            result.Title.Should().Be(post.Title);
        }
    }
}
