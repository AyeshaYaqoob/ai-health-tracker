namespace HealthTracker.Application.Features.WeeklyReports.DTOs;

public record WeeklyReportDto(
    Guid Id,
    DateOnly WeekStartDate,
    DateOnly WeekEndDate,
    string AiInsights,
    string TopSymptoms,
    float AvgMoodScore,
    float AvgSleepHours,
    DateTime GeneratedAt
);