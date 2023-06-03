using AutoFixture;
using DbService.Interfaces;
using DbService.Models;
using DbService.Repositories;
using DbService.Services;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Moq;
using Serilog;

namespace UnitTests.DatabaseService
{
    [TestFixture]
    public class ImageServiceTests : BaseTest
    {
        private Mock<IGridFsService> _gridFsMock;
        private Mock<IRepository<Image>> _imageRepositoryMock;
        private Mock<IHistoryService> _historyServiceMock;
        private Mock<ILogger> _loggerMock;
        private Mock<ISettingsService> _settingsServiceMock;

        private IImageService _imageService;

        [SetUp]
        public void Setup()
        {
            _gridFsMock = new Mock<IGridFsService>();
            _imageRepositoryMock = new Mock<IRepository<Image>>();
            _historyServiceMock = new Mock<IHistoryService>();
            _settingsServiceMock = new Mock<ISettingsService>();
            _loggerMock = new Mock<ILogger>();

            _imageService = new ImageService(_gridFsMock.Object, _imageRepositoryMock.Object,
                _historyServiceMock.Object, _settingsServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public void SaveImage_ShouldReturn_True()
        {
            var objectId = ObjectId.GenerateNewId();

            var imageStream = Fixture.Create<MemoryStream>();
            var image = Fixture.Create<GrpcHelper.DbService.Image>();

            _gridFsMock.Setup(s => s.AddFileAsStream(It.IsAny<MemoryStream>(), It.IsAny<string>(),
                It.IsAny<GridFSUploadOptions>(), CancellationToken.None)).ReturnsAsync(objectId);

            _imageRepositoryMock.Setup(s =>
                s.Insert(It.IsAny<Image>(), It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(true);

            var result = _imageService.SaveImage(imageStream, image)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Status.Should().BeTrue();
        }

        [Test]
        public void SaveImage_ShouldReturn_False()
        {
            var objectId = ObjectId.Empty;

            var imageStream = Fixture.Create<MemoryStream>();
            var image = Fixture.Create<GrpcHelper.DbService.Image>();

            _gridFsMock.Setup(s => s.AddFileAsStream(It.IsAny<MemoryStream>(), It.IsAny<string>(),
                It.IsAny<GridFSUploadOptions>(), CancellationToken.None)).ReturnsAsync(objectId);

            var result = _imageService.SaveImage(imageStream, image)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);
            _imageRepositoryMock.Verify(v => v.Insert(It.IsAny<Image>(), It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Never);

            result.Status.Should().BeFalse();
        }

        [Test]
        public void GetImageById_ShouldReturn_Image()
        {
            var objectId = Fixture.Create<ObjectId>();

            var imageStream = Fixture.Create<MemoryStream>();
            var image = Fixture.Build<Image>()
                .With(w => w.GridFsId, objectId)
                .Create();

            _imageRepositoryMock
                .Setup(s => s.Find(It.IsAny<FilterDefinition<Image>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(image);

            _gridFsMock.Setup(s => s.GetFileAsStream(objectId, It.IsAny<GridFSDownloadOptions>(), CancellationToken.None))
                .ReturnsAsync(imageStream);

            var result = _imageService.GetImageById(objectId)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.image.Should().NotBeNull();
            result.image.GridFsId.Should().Be(objectId);
            result.stream.Should().NotBeNull();
        }

        [Test]
        public void GetImagesByTags_ShouldReturn_Image()
        {
            var objectId = Fixture.Create<ObjectId>();
            var settingsId = Fixture.Create<string>();
            var settings = Fixture.Create<PosterSettings>();

            var imageStream = Fixture.Create<MemoryStream>();
            var images = Fixture.Build<Image>()
                .With(w => w.GridFsId, objectId)
                .CreateMany(5)
                .ToList();

            _settingsServiceMock
                .Setup(s => s.GetPosterSetting(settingsId))
                .ReturnsAsync(settings);

            _imageRepositoryMock
                .Setup(s => s.FindMany(It.IsAny<FilterDefinition<Image>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(images);

            _gridFsMock.Setup(s => s.GetFileAsStream(objectId, It.IsAny<GridFSDownloadOptions>(), CancellationToken.None))
                .ReturnsAsync(imageStream);

            var result = _imageService.GetImagesBySettingId(settingsId)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Select(s => s.image.GridFsId).Should().AllBeEquivalentTo(objectId);
        }
    }
}
