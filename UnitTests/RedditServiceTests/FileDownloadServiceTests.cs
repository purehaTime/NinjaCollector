using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using RedditService.Interfaces;
using RedditService.Services;
using Serilog;
using System.Net;

namespace UnitTests.RedditServiceTests
{
    [TestFixture]
    public class FileDownloadServiceTests : BaseTest
    {
        private Mock<IHttpClientFactory> _clientFactoryMock;
        private Mock<ILogger> _loggerMock;
        private Mock<HttpMessageHandler> _httpHandler;

        private IFileDownloadService _fileDownloadService;


        [OneTimeSetUp]
        public void Init()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger>();
            _httpHandler = new Mock<HttpMessageHandler>();

            _fileDownloadService = new FileDownloadService(_clientFactoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetData_ShouldReturn_HttpStringData()
        {
            var link = "http://example.com";
            var data = Fixture.Create<string>();
            _httpHandler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK, data);

            var client = _httpHandler.CreateClient();

            _clientFactoryMock.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(() => client);

            var result = _fileDownloadService.GetData(link)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Should().Be(data);
        }

        [Test]
        public void GetFiles_ShouldReturn_ListBytesArray()
        {
            var links = new List<string> { "http://example.com" };
            var data = Fixture.Create<byte[]>();

            _httpHandler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK, data);
            var client = _httpHandler.CreateClient();

            _clientFactoryMock.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(() => client);

            var result = _fileDownloadService.GetFiles(links)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Should().AllBeOfType<byte[]>();
            result.Should().AllBeEquivalentTo(data);
        }

        [Test]
        public void GetFile_ShouldReturn_ByteArray()
        {
            var link = "http://example.com";
            var data = Fixture.Create<byte[]>();
            _httpHandler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK, data);

            var client = _httpHandler.CreateClient();

            _clientFactoryMock.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(() => client);

            var result = _fileDownloadService.GetFile(link)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Should().AllBeOfType<byte>();
            result.Should().BeEquivalentTo(data);
        }
    }
}
