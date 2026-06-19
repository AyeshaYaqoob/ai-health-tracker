namespace HealthTracker.Application.Features.Insights.DTOs;

public record HealthInsightsDto(
    string Insights,
    DateOnly From,
    DateOnly To,
    int TotalLogsAnalyzed,
    DateTime GeneratedAt
);