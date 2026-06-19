using HealthTracker.Application.Features.MealLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Commands;

public record UpdateMealLogCommand(
    Guid Id,
    Guid UserId,
    string MealType,
    string Description
) : IRequest<MealLogDto>;