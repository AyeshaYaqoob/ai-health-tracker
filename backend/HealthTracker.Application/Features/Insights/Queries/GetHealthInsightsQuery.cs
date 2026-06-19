using HealthTracker.Application.Features.Insights.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.Insights.Queries;

public record GetHealthInsightsQuery(
    Guid UserId,
    DateOnly From,
    DateOnly To
) : IRequest<HealthInsightsDto>;