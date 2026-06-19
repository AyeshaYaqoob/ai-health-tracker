using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Commands;

public record DeleteMoodLogCommand(
    Guid Id,
    Guid UserId
) : IRequest<bool>;