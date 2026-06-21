using FluentAssertions;
using HealthTracker.Application.Features.MoodLogs.Commands;
using HealthTracker.Application.Features.MoodLogs.Handlers;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Moq;

namespace HealthTracker.Tests.Unit.Handlers;

public class MoodLogHandlerTests
{
    private readonly Mock<IMoodLogRepository> _repoMock = new();
    private readonly CreateMoodLogHandler _handler;

    public MoodLogHandlerTests()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<MoodLog>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _handler = new CreateMoodLogHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidMoodScore_ReturnsMoodLogDto()
    {
        // Arrange
        var command = new CreateMoodLogCommand(Guid.NewGuid(), 7, "Feeling good", DateOnly.FromDateTime(DateTime.Today));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.MoodScore.Should().Be(7);
        result.Notes.Should().Be("Feeling good");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    [InlineData(-1)]
    public async Task Handle_InvalidMoodScore_ThrowsArgumentException(int invalidScore)
    {
        // Arrange
        var command = new CreateMoodLogCommand(Guid.NewGuid(), invalidScore, null, DateOnly.FromDateTime(DateTime.Today));

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Mood score must be between 1 and 10.");
    }

    [Fact]
    public async Task Handle_ValidMoodLog_PersistsToRepository()
    {
        // Arrange
        MoodLog? savedLog = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<MoodLog>()))
            .Callback<MoodLog>(l => savedLog = l)
            .Returns(Task.CompletedTask);

        var userId = Guid.NewGuid();
        var command = new CreateMoodLogCommand(userId, 8, "Great day!", DateOnly.FromDateTime(DateTime.Today));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedLog.Should().NotBeNull();
        savedLog!.UserId.Should().Be(userId);
        savedLog.MoodScore.Should().Be(8);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Handle_BoundaryMoodScores_Succeed(int score)
    {
        // Arrange
        var command = new CreateMoodLogCommand(Guid.NewGuid(), score, null, DateOnly.FromDateTime(DateTime.Today));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.MoodScore.Should().Be(score);
    }
}
