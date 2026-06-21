using FluentAssertions;
using HealthTracker.Application.Features.SleepLogs.Commands;
using HealthTracker.Application.Features.SleepLogs.Handlers;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Moq;

namespace HealthTracker.Tests.Unit.Handlers;

public class SleepLogHandlerTests
{
    private readonly Mock<ISleepLogRepository> _repoMock = new();
    private readonly CreateSleepLogHandler _handler;

    private static readonly TimeOnly DefaultBedTime = new(23, 0);
    private static readonly TimeOnly DefaultWakeTime = new(7, 0);
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

    public SleepLogHandlerTests()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<SleepLog>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _handler = new CreateSleepLogHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidSleepLog_ReturnsSleepLogDto()
    {
        // Arrange
        var command = new CreateSleepLogCommand(Guid.NewGuid(), 8.0f, 9, DefaultBedTime, DefaultWakeTime, Today);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HoursSlept.Should().Be(8.0f);
        result.QualityScore.Should().Be(9);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    [InlineData(-5)]
    public async Task Handle_InvalidQualityScore_ThrowsArgumentException(int invalidScore)
    {
        // Arrange
        var command = new CreateSleepLogCommand(Guid.NewGuid(), 7.5f, invalidScore, DefaultBedTime, DefaultWakeTime, Today);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Quality score must be between 1 and 10.");
    }

    [Fact]
    public async Task Handle_ValidSleepLog_PersistsWithCorrectUserId()
    {
        // Arrange
        SleepLog? savedLog = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<SleepLog>()))
            .Callback<SleepLog>(l => savedLog = l)
            .Returns(Task.CompletedTask);

        var userId = Guid.NewGuid();
        var command = new CreateSleepLogCommand(userId, 7.5f, 8, DefaultBedTime, DefaultWakeTime, Today);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedLog.Should().NotBeNull();
        savedLog!.UserId.Should().Be(userId);
        savedLog.HoursSlept.Should().Be(7.5f);
        savedLog.BedTime.Should().Be(DefaultBedTime);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Handle_BoundaryQualityScores_Succeed(int score)
    {
        // Arrange
        var command = new CreateSleepLogCommand(Guid.NewGuid(), 7.0f, score, DefaultBedTime, DefaultWakeTime, Today);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.QualityScore.Should().Be(score);
    }
}
