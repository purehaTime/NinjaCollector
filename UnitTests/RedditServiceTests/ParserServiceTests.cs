using System.ComponentModel.DataAnnotations;
using AutoFixture;
using FluentAssertions;
using ModelsHelper.Models;
using Moq;
using Newtonsoft.Json.Linq;
using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;
using RedditService.Services;
using RestSharp;
using Worker.Model;

namespace UnitTests.RedditServiceTests
{
    [TestFixture]
    public class ParserServiceTests : BaseTest
    {
        private Mock<IParserGalleryService> _parserGalleryServiceMock;
        private Mock<IFileDownloadService> _fileDownloadServiceMock;
        private Mock<IFilterService> _filterServiceMock;

        private IParserService _parserService;


        [OneTimeSetUp]
        public void Init()
        {
            _parserGalleryServiceMock = new Mock<IParserGalleryService>();
            _fileDownloadServiceMock = new Mock<IFileDownloadService>();
            _filterServiceMock = new Mock<IFilterService>();

            _parserService = new ParserService(_parserGalleryServiceMock.Object, _fileDownloadServiceMock.Object, _filterServiceMock.Object);
        }

        [Test]
        public void ParsePost_ShouldReturn_Null()
        {
            var link = "v.redd.it";
            var post = Fixture.Create<Post>();
            post.Listing.Domain = link;

            var filter = Fixture.Create<Filter>();
            _filterServiceMock.Setup(s => s.IsValid(post, filter)).Returns(false);

            var result = _parserService.ParsePost(post, filter)
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
            var filter = Fixture.Create<Filter>();
            var bytes = Fixture.Create<byte[]>();

            _filterServiceMock.Setup(s => s.IsValid(post, filter)).Returns(true);
            _parserGalleryServiceMock.Setup(s => s.GetImageLinks(link)).ReturnsAsync(images);
            _fileDownloadServiceMock.Setup(s => s.GetFile(It.IsAny<string>())).ReturnsAsync(bytes);

            var result = _parserService.ParsePost(post, filter)
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
            var filter = Fixture.Create<Filter>();
            var post = Fixture.Create<Post>();
            post.Listing.URL = link;
            post.Listing.Preview = JObject.Parse("{\"images\":[{\"source\":{\"url\":\"https://test.com\",\"width\":32,\"height\":64}}]}");

            _fileDownloadServiceMock.Setup(s => s.GetFile(It.IsAny<string>())).ReturnsAsync(bytes);
            _filterServiceMock.Setup(s => s.IsValid(post, filter)).Returns(true);

            var result = _parserService.ParsePost(post, filter)
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
