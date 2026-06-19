using HealthTracker.Application.Features.SleepLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Queries;

public record GetSleepLogsQuery(
    Guid UserId,
    DateOnly? From,
    DateOnly? To
) : IRequest<List<SleepLogDto>>;