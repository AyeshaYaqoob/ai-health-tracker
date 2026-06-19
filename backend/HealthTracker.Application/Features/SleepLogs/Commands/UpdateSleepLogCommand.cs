using HealthTracker.Application.Features.SleepLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Commands;

public record UpdateSleepLogCommand(
    Guid Id,
    Guid UserId,
    float HoursSlept,
    int QualityScore,
    TimeOnly BedTime,
    TimeOnly WakeTime
) : IRequest<SleepLogDto>;