using HealthTracker.Application.Features.MealLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Commands;

public record CreateMealLogCommand(
    Guid UserId,
    string MealType,
    string Description,
    DateOnly LogDate
) : IRequest<MealLogDto>;