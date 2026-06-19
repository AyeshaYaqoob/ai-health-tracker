using HealthTracker.Application.Features.MealLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Queries;

public record GetMealLogsQuery(
    Guid UserId,
    DateOnly? From,
    DateOnly? To
) : IRequest<List<MealLogDto>>;