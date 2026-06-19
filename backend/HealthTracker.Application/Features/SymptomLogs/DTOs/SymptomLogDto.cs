namespace HealthTracker.Application.Features.SymptomLogs.DTOs;

public record SymptomLogDto(
    Guid Id,
    string SymptomName,
    int Severity,
    string? Notes,
    DateOnly LogDate,
    DateTime CreatedAt
);