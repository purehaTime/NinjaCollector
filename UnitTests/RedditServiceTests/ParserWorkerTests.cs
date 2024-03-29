﻿using AutoFixture;
using FluentAssertions;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using Moq;
using RedditService.Interfaces;
using RedditService.Workers;
using Serilog;
using Worker.Interfaces;
using Filter = ModelsHelper.Models.Filter;
using ParserSettings = ModelsHelper.Models.ParserSettings;
using Post = ModelsHelper.Models.Post;

namespace UnitTests.RedditServiceTests
{
    [TestFixture]
    public class ParserWorkerTests : BaseTest
    {
        private Mock<IRedditService> _redditServiceMock;
        private Mock<IDatabaseServiceClient> _dbServiceClientMock;
        private Mock<ILogger> _loggerMock;

        private IWorker _parserWorker;

        [OneTimeSetUp]
        public void Setup()
        {
            _redditServiceMock = new Mock<IRedditService>();
            _dbServiceClientMock = new Mock<IDatabaseServiceClient>();
            _loggerMock = new Mock<ILogger>();

            _parserWorker = new ParserWorker(_redditServiceMock.Object, _dbServiceClientMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Name_ShouldBe_RedditParser()
        {
            _parserWorker.Name.Should().Be("Reddit_Parser");
        }

        [Test]
        public void Init_ShouldReturn_SettingsList()
        {
            var parserSettings = Fixture.Build<ParserSettingsModel>()
                .With(w => w.Disabled, false)
                .CreateMany(5)
                .ToList();

            _dbServiceClientMock.Setup(s => s.GetParserSettings(It.IsAny<ParserSettingsRequest>()))
                .ReturnsAsync(parserSettings);

            var result = _parserWorker.Init()
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }

        [Test]
        public void LoadSettings_ShouldReturn_Settings()
        {
            var parserSettings = Fixture.Build<ParserSettingsModel>()
                .With(w => w.Disabled, false)
                .CreateMany(5)
                .ToList();

            _dbServiceClientMock.Setup(s => s.GetParserSettings(It.IsAny<ParserSettingsRequest>()))
                .ReturnsAsync(parserSettings);

            var result = _parserWorker.LoadSettings(parserSettings.First().Id)
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Id.Should().Be(parserSettings.First().Id);
        }

        [Test]
        public void Run_ShouldReturn_UpdatedSettings_WithLastPostId()
        {
            var subName = Fixture.Create<string>();
            var filter = Fixture.Create<Filter>();
            var settings = Fixture.Build<ParserSettings>()
                .With(w => w.ByLastPostId, true)
                .With(w => w.Group, subName)
                .With(w => w.ContinueMonitoring, true)
                .With(w => w.FromDate, DateTime.UtcNow)
                .With(w => w.UntilDate, DateTime.UtcNow)
                .With(w => w.Disabled, false)
                .With(w => w.Filter, filter)
                .Create();

            var contents = Fixture.Build<Post>()
                .With(w => w.PostDate, DateTime.UtcNow)
                .CreateMany(5)
                .ToList();

            _redditServiceMock.Setup(s => s.GetPostsUntilPostId(subName, It.IsAny<string>(), filter))
                .ReturnsAsync(contents);

            _dbServiceClientMock.Setup(s => s.AddPosts(It.IsAny<List<GrpcHelper.DbService.Post>>())).ReturnsAsync(true);
            _dbServiceClientMock.Setup(s => s.SaveParserSettings(It.IsAny<ParserSettingsModel>())).ReturnsAsync(true);

            var result = _parserWorker.Run(settings)
                .GetAwaiter()
                .GetResult();

            _redditServiceMock.Verify(v => v.GetPostsUntilPostId(subName, It.IsAny<string>(), filter), Times.Once);
            _redditServiceMock.Verify(v => v.GetPostsBetweenDates(subName, It.IsAny<DateTime>(), It.IsAny<DateTime>(), filter), Times.Never);

            result.Should().NotBeNull();
            result.Should().BeOfType<ParserSettings>();
            (result as ParserSettings)?.UntilPostId.Should().Be(contents.First().PostId);
            result.Disabled.Should().Be(false);
        }

        [Test]
        public void Run_ShouldReturn_UpdatedSettings_WithDisabled()
        {
            var subName = Fixture.Create<string>();
            var filter = Fixture.Create<Filter>();

            var settings = Fixture.Build<ParserSettings>()
                .With(w => w.ByLastPostId, true)
                .With(w => w.Group, subName)
                .With(w => w.ContinueMonitoring, false)
                .With(w => w.FromDate, DateTime.UtcNow)
                .With(w => w.UntilDate, DateTime.UtcNow)
                .With(w => w.Disabled, false)
                .With(w => w.Filter, filter)
                .Create();

            var contents = Fixture.Build<Post>()
                .With(w => w.PostDate, DateTime.UtcNow)
                .CreateMany(5)
                .ToList();

            _redditServiceMock.Setup(s => s.GetPostsUntilPostId(subName, It.IsAny<string>(), filter))
                .ReturnsAsync(contents);

            _dbServiceClientMock.Setup(s => s.AddPosts(It.IsAny<List<GrpcHelper.DbService.Post>>())).ReturnsAsync(true);
            _dbServiceClientMock.Setup(s => s.SaveParserSettings(It.IsAny<ParserSettingsModel>())).ReturnsAsync(true);

            var result = _parserWorker.Run(settings)
                .GetAwaiter()
                .GetResult();

            _redditServiceMock.Verify(v => v.GetPostsUntilPostId(subName, It.IsAny<string>(), filter), Times.Once);
            _redditServiceMock.Verify(v => v.GetPostsBetweenDates(subName, It.IsAny<DateTime>(), It.IsAny<DateTime>(), filter), Times.Never);

            result.Should().NotBeNull();
            result.Should().BeOfType<ParserSettings>();
            (result as ParserSettings)?.Should().NotBe(contents.First().PostId);
            result.Disabled.Should().Be(true);
        }
    }
}
