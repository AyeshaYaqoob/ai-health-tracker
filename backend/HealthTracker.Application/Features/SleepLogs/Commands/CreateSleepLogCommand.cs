using HealthTracker.Application.Features.SleepLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Commands;

public record CreateSleepLogCommand(
    Guid UserId,
    float HoursSlept,
    int QualityScore,
    TimeOnly BedTime,
    TimeOnly WakeTime,
    DateOnly LogDate
) : IRequest<SleepLogDto>;