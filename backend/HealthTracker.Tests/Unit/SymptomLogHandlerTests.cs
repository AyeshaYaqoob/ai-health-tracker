using FluentAssertions;
using HealthTracker.Application.Features.SymptomLogs.Commands;
using HealthTracker.Application.Features.SymptomLogs.Handlers;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Moq;

namespace HealthTracker.Tests.Unit;

public class SymptomLogHandlerTests
{
    private readonly Mock<ISymptomLogRepository> _repoMock = new();
    private readonly CreateSymptomLogHandler _handler;

    public SymptomLogHandlerTests()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<SymptomLog>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _handler = new CreateSymptomLogHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidSymptomLog_ReturnsSymptomLogDto()
    {
        // Arrange
        var command = new CreateSymptomLogCommand(
            Guid.NewGuid(), "Headache", 6, "After long screen time",
            DateOnly.FromDateTime(DateTime.Today));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SymptomName.Should().Be("Headache");
        result.Severity.Should().Be(6);
        result.Notes.Should().Be("After long screen time");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    [InlineData(-1)]
    public async Task Handle_InvalidSeverity_ThrowsArgumentException(int invalidSeverity)
    {
        // Arrange
        var command = new CreateSymptomLogCommand(
            Guid.NewGuid(), "Migraine", invalidSeverity, null,
            DateOnly.FromDateTime(DateTime.Today));

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Severity must be between 1 and 10.");
    }

    [Fact]
    public async Task Handle_ValidSymptomLog_PersistsToRepository()
    {
        // Arrange
        SymptomLog? savedLog = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<SymptomLog>()))
            .Callback<SymptomLog>(l => savedLog = l)
            .Returns(Task.CompletedTask);

        var userId = Guid.NewGuid();
        var command = new CreateSymptomLogCommand(
            userId, "Fatigue", 4, "After lunch",
            DateOnly.FromDateTime(DateTime.Today));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedLog.Should().NotBeNull();
        savedLog!.UserId.Should().Be(userId);
        savedLog.SymptomName.Should().Be("Fatigue");
        savedLog.Severity.Should().Be(4);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Handle_BoundarySeverity_Succeeds(int severity)
    {
        // Arrange
        var command = new CreateSymptomLogCommand(
            Guid.NewGuid(), "Nausea", severity, null,
            DateOnly.FromDateTime(DateTime.Today));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Severity.Should().Be(severity);
    }

    [Fact]
    public async Task Handle_NullNotes_SucceedsAndReturnsNullNotes()
    {
        // Arrange
        var command = new CreateSymptomLogCommand(
            Guid.NewGuid(), "Dizziness", 3, null,
            DateOnly.FromDateTime(DateTime.Today));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Notes.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ValidLog_CallsSaveChangesExactlyOnce()
    {
        // Arrange
        var command = new CreateSymptomLogCommand(
            Guid.NewGuid(), "BackPain", 7, null,
            DateOnly.FromDateTime(DateTime.Today));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
