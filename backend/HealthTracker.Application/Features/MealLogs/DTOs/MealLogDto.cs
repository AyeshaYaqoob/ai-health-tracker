namespace HealthTracker.Application.Features.MealLogs.DTOs;

public record MealLogDto(
    Guid Id,
    string MealType,
    string Description,
    DateOnly LogDate,
    DateTime CreatedAt
);