using AutoFixture;
using DbService.Interfaces;
using DbService.Models;
using DbService.Services;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Moq;
using Serilog;

namespace UnitTests.DatabaseService
{
    public class ImageServiceTests : BaseTest
    {
        private Mock<IGridFsRepository> _gridFsMock;
        private Mock<IRepository<Image>> _imageRepositoryMock;
        private Mock<IHistoryService> _historyServiceMock;
        private Mock<ILogger> _loggerMock;

        private IImageService _imageService;

        [SetUp]
        public void Setup()
        {
            _gridFsMock = new Mock<IGridFsRepository>();
            _imageRepositoryMock = new Mock<IRepository<Image>>();
            _historyServiceMock = new Mock<IHistoryService>();
            _loggerMock = new Mock<ILogger>();
        }

        [Test]
        public void SaveImage_ShouldReturn_True()
        {
            var objectId = ObjectId.GenerateNewId();

            var imageStream = Fixture.Create<MemoryStream>();
            var image = Fixture.Build<Image>()
                .Create();

            _gridFsMock.Setup(s => s.AddFileAsStream(It.IsAny<MemoryStream>(), It.IsAny<string>(),
                It.IsAny<GridFSUploadOptions>(), CancellationToken.None)).ReturnsAsync(objectId);

            _imageRepositoryMock.Setup(s =>
                s.Insert(It.IsAny<Image>(), It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(true);

            _imageService = new ImageService(_gridFsMock.Object, _imageRepositoryMock.Object,
                _historyServiceMock.Object, _loggerMock.Object);

            var result = _imageService.SaveImage(imageStream, image)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().BeTrue();
        }

        [Test]
        public void SaveImage_ShouldReturn_False()
        {
            var objectId = ObjectId.Empty;

            var imageStream = Fixture.Create<MemoryStream>();
            var image = Fixture.Build<Image>()
                .Create();

            _gridFsMock.Setup(s => s.AddFileAsStream(It.IsAny<MemoryStream>(), It.IsAny<string>(),
                It.IsAny<GridFSUploadOptions>(), CancellationToken.None)).ReturnsAsync(objectId);

            _imageService = new ImageService(_gridFsMock.Object, _imageRepositoryMock.Object,
                _historyServiceMock.Object, _loggerMock.Object);

            var result = _imageService.SaveImage(imageStream, image)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);
            _imageRepositoryMock.Verify(v => v.Insert(It.IsAny<Image>(), It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Never);

            result.Should().BeFalse();
        }
    }
}
