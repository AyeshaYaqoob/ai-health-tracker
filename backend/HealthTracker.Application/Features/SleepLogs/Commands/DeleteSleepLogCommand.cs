using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Commands;

public record DeleteSleepLogCommand(
    Guid Id,
    Guid UserId
) : IRequest<bool>;