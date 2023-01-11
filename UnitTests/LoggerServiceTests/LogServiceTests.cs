using AutoFixture;
using FluentAssertions;
using GrpcHelper.LogService;
using LoggerService.Interfaces;
using LoggerService.Services;
using LoggerService.Models;
using Moq;
using Serilog;
using Log = LoggerService.Models.Log;

namespace UnitTests.LoggerServiceTests
{
    public class LogServiceTests : BaseTest
    {
        private Mock<IDatabase> _dbMock = null!;
        private Mock<ILogger> _loggerMock = null!;

        private LogService _logService = null!;

        [SetUp]
        public void Setup()
        {
            _dbMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger>();
        }

        [Test]
        public void WriteLog_ShouldReturn_SuccessTrue()
        {
            var logModel = Fixture.Create<LogModel>();

            _dbMock.Setup(s => s.Add(It.IsAny<object>())).ReturnsAsync(true);
            _logService = new LogService(_dbMock.Object, _loggerMock.Object);

            var result = _logService.WriteLog(logModel, null!)
                .GetAwaiter()
                .GetResult();

            _dbMock.Verify(v => v.Add(It.IsAny<object>()), Times.Once);
            _loggerMock.Verify(v => v.Fatal(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);

            result.Success.Should().BeTrue();
        }

        [Test]
        public void WriteLog_ShouldReturn_SuccessFalse()
        {
            var logModel = Fixture.Create<LogModel>();

            _dbMock.Setup(s => s.Add(It.IsAny<object>())).ReturnsAsync(false);
            _logService = new LogService(_dbMock.Object, _loggerMock.Object);

            var result = _logService.WriteLog(logModel, null!)
                .GetAwaiter()
                .GetResult();

            _dbMock.Verify(v => v.Add(It.IsAny<object>()), Times.Once);
            _loggerMock.Verify(v => v.Fatal(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);

            result.Success.Should().BeFalse();
        }

        [Test]
        public void WriteLog_ShouldInterrupt_ByException()
        {
            var logModel = Fixture.Create<LogModel>();

            _dbMock.Setup(s => s.Add(It.IsAny<object>())).ThrowsAsync(new Exception("error"));
            _logService = new LogService(_dbMock.Object, _loggerMock.Object);

            var result = _logService.WriteLog(logModel, null!)
                .GetAwaiter()
                .GetResult();

            _dbMock.Verify(v => v.Add(It.IsAny<object>()), Times.Once);
            _loggerMock.Verify(v => v.Fatal(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);

            result.Success.Should().BeFalse();
        }

        [Test]
        public void GetAll_ShouldReturn_LogsResponse()
        {
            var logs = Fixture.Build<Log>()
                .With(w => w.Timestamp, new DateTime().ToUniversalTime())
                .CreateMany(5)
                .ToList();

            _dbMock.Setup(s => s.GetAll<Log>()).ReturnsAsync(logs);
            _logService = new LogService(_dbMock.Object, _loggerMock.Object);

            var result = _logService.GetLogs(It.IsAny<LogsRequest>(), null!)
                .GetAwaiter()
                .GetResult();

            _dbMock.Verify(v => v.GetAll<Log>(), Times.Once);
            _loggerMock.Verify(v => v.Fatal(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.Logs.Should().NotBeNull();
            result.Logs.Should().HaveCount(logs.Count);
        }

        [Test]
        public void GetAll_ShouldInterrupt_ByException()
        {
            // Apply
            var logs = Fixture.Build<Log>()
                .With(w => w.Timestamp, new DateTime().ToUniversalTime())
                .CreateMany(5)
                .ToList();

            _dbMock.Setup(s => s.GetAll<Log>()).ThrowsAsync(new Exception("error"));
            _logService = new LogService(_dbMock.Object, _loggerMock.Object);

            // Action
            var result = _logService.GetLogs(It.IsAny<LogsRequest>(), null!)
                .GetAwaiter()
                .GetResult();

            // Assert
            _dbMock.Verify(v => v.GetAll<Log>(), Times.Once);
            _loggerMock.Verify(v => v.Fatal(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);

            result.Should().BeNull();
        }
    }
}
