namespace HealthTracker.Application.Features.SleepLogs.DTOs;

public record SleepLogDto(
    Guid Id,
    float HoursSlept,
    int QualityScore,
    TimeOnly BedTime,
    TimeOnly WakeTime,
    DateOnly LogDate,
    DateTime CreatedAt
);