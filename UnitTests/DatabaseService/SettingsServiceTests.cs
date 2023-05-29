using AutoFixture;
using DbService.Interfaces;
using DbService.Models;
using DbService.Services;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace UnitTests.DatabaseService
{
    [TestFixture]
    public class SettingsServiceTests : BaseTest
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IRepository<ParserSettings>> _parserRepositoryMock;
        private Mock<IRepository<PosterSettings>> _posterRepositoryMock;

        private ISettingsService _settingsService;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger>();
            _parserRepositoryMock = new Mock<IRepository<ParserSettings>>();
            _posterRepositoryMock = new Mock<IRepository<PosterSettings>>();
        }

        [Test]
        public void SaveParserSettings_ShouldReturn_True()
        {
            var parser = Fixture.Create<ParserSettings>();

            _parserRepositoryMock.Setup(s => s.Insert(parser, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(true);

            _settingsService = new SettingsService(_parserRepositoryMock.Object, _posterRepositoryMock.Object,
                _loggerMock.Object);

            var result = _settingsService.SaveParserSettings(parser)
                .GetAwaiter()
                .GetResult();

            _parserRepositoryMock.Verify(v => v.Insert(parser, It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Once);
            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().BeTrue();
        }

        [Test]
        public void SaveParserSettings_ShouldReturn_False()
        {
            var parser = Fixture.Create<ParserSettings>();

            _parserRepositoryMock.Setup(s => s.Insert(parser, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(false);

            _settingsService = new SettingsService(_parserRepositoryMock.Object, _posterRepositoryMock.Object,
                _loggerMock.Object);

            var result = _settingsService.SaveParserSettings(parser)
                .GetAwaiter()
                .GetResult();

            _parserRepositoryMock.Verify(v => v.Insert(parser, It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Once);
            result.Should().BeFalse();
        }

        [Test]
        public void SavePosterSettings_ShouldReturn_True()
        {
            var poster = Fixture.Create<PosterSettings>();

            _posterRepositoryMock.Setup(s => s.Insert(poster, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(true);

            _settingsService = new SettingsService(_parserRepositoryMock.Object, _posterRepositoryMock.Object,
                _loggerMock.Object);

            var result = _settingsService.SavePosterSettings(poster)
                .GetAwaiter()
                .GetResult();

            _posterRepositoryMock.Verify(v => v.Insert(poster, It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Once);
            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().BeTrue();
        }

        [Test]
        public void SavePosterSettings_ShouldReturn_False()
        {
            var poster = Fixture.Create<PosterSettings>();

            _posterRepositoryMock.Setup(s => s.Insert(poster, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(false);

            _settingsService = new SettingsService(_parserRepositoryMock.Object, _posterRepositoryMock.Object,
                _loggerMock.Object);

            var result = _settingsService.SavePosterSettings(poster)
                .GetAwaiter()
                .GetResult();

            _posterRepositoryMock.Verify(v => v.Insert(poster, It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Once);
            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);

            result.Should().BeFalse();
        }

        [Test]
        public void GetParserSettings_ShouldReturn_ParserSettings()
        {
            var source = Fixture.Create<string>();
            var parserSettings = Fixture.CreateMany<ParserSettings>(5).ToList();

            _parserRepositoryMock
                .Setup(s => s.FindMany(It.IsAny<FilterDefinition<ParserSettings>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(parserSettings);

            _settingsService = new SettingsService(_parserRepositoryMock.Object, _posterRepositoryMock.Object,
                _loggerMock.Object);

            var result = _settingsService.GetParserSettings(source, null)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeEmpty();
            result.Should().HaveCount(5);
            result.Should().IntersectWith(parserSettings);
        }

        [Test]
        public void GetPosterSettings_ShouldReturn_PosterSettings()
        {
            var service = Fixture.Create<string>();
            var settingId = Fixture.Create<ObjectId>();
            var posterSettings = Fixture.CreateMany<PosterSettings>(5).ToList();

            _posterRepositoryMock
                .Setup(s => s.FindMany(It.IsAny<FilterDefinition<PosterSettings>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(posterSettings);

            _settingsService = new SettingsService(_parserRepositoryMock.Object, _posterRepositoryMock.Object,
                _loggerMock.Object);

            var result = _settingsService.GetPosterSettings(service, settingId)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeEmpty();
            result.Should().HaveCount(5);
            result.Should().IntersectWith(posterSettings);
        }
    }
}
