using HealthTracker.Application.Features.SleepLogs.DTOs;
using HealthTracker.Application.Features.SleepLogs.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Handlers;

public class GetSleepLogsHandler : IRequestHandler<GetSleepLogsQuery, List<SleepLogDto>>
{
    private readonly ISleepLogRepository _repository;
    public GetSleepLogsHandler(ISleepLogRepository repository) => _repository = repository;

    public async Task<List<SleepLogDto>> Handle(GetSleepLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByUserIdAsync(request.UserId, request.From, request.To);
        return logs.Select(l => new SleepLogDto(l.Id, l.HoursSlept, l.QualityScore, l.BedTime, l.WakeTime, l.LogDate, l.CreatedAt)).ToList();
    }
}