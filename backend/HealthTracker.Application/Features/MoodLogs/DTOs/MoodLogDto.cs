namespace HealthTracker.Application.Features.MoodLogs.DTOs;

public record MoodLogDto(
    Guid Id,
    int MoodScore,
    string? Notes,
    DateOnly LogDate,
    DateTime CreatedAt
);