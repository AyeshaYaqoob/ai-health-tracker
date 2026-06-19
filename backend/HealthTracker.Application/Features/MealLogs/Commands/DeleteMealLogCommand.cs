using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Commands;

public record DeleteMealLogCommand(
    Guid Id,
    Guid UserId
) : IRequest<bool>;