using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using RedditService.Interfaces;
using RedditService.Services;
using Serilog;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.RedditService
{
    [TestFixture]
    public class ParserGalleryServiceTests : BaseTest
    {
        private Mock<IFileDownloadService> _fileDownloadServiceMock;
        private Mock<ILogger> _loggerMock;

        private IParserGalleryService _parserGalleryService;


        [OneTimeSetUp]
        public void Init()
        {
            _fileDownloadServiceMock = new Mock<IFileDownloadService>();
            _loggerMock = new Mock<ILogger>();

            _parserGalleryService = new ParserGalleryService(_fileDownloadServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetData_ShouldReturn_HttpStringData()
        {
            var link = "http://example.com";

            _fileDownloadServiceMock.Setup(s => s.GetData(It.IsAny<string>())).ReturnsAsync(_testJson);

            var result = _parserGalleryService.GetImageLinks(link)
                .GetAwaiter()
                .GetResult().ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var firstImage = result.First();
            firstImage.DirectLink.Should().Be("https://test1.com");
            firstImage.Name.Should().Be("test1");
            firstImage.Description.Should().Be("testcaption");
        }

        [Test]
        public void GetData_ShouldReturn_HttpStringData11()
        {
            var link = "https://www.reddit.com/gallery/13vj8x9";


            var client = new HttpClient();
            //_parserGalleryService = new ParserGalleryService(new FileDownloadService(client ), _loggerMock.Object);

            var result = _parserGalleryService.GetImageLinks(link)
                .GetAwaiter()
                .GetResult().ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var firstImage = result.First();
            firstImage.DirectLink.Should().Be("https://test1.com");
            firstImage.Name.Should().Be("test1");
            firstImage.Description.Should().Be("testcaption");
        }


        private string _testJson = "[\r\n{\r\n\"data\":{\r\n\"children\":[\r\n{\r\n\"data\":{\r\n\"media_metadata\":{\r\n\"test1\":{\r\n\"status\":\"valid\",\r\n\"e\":\"Image\",\r\n\"m\":\"image/jpg\",\r\n\"p\":[\r\n{\r\n\"y\":71,\r\n\"x\":108,\r\n\"u\":\"https://test1.com\"\r\n},\r\n{\r\n\"y\":143,\r\n\"x\":216,\r\n\"u\":\"https://test1.com\"\r\n}\r\n],\r\n\"s\":{\r\n\"y\":682,\r\n\"x\":1024,\r\n\"u\":\"https://test1.com\"\r\n},\r\n\"id\":\"test1\"\r\n}\r\n},\r\n\"gallery_data\":{\r\n\"items\":[\r\n{\r\n\"caption\":\"testcaption\",\r\n\"outbound_url\":\"outbound_url\",\r\n\"media_id\":\"test1\",\r\n\"id\":123\r\n}\r\n]\r\n},\r\n}\r\n}\r\n],\r\n\"before\":null\r\n}\r\n}\r\n]";
    }
}
