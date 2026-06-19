using HealthTracker.Application.Features.MoodLogs.DTOs;
using HealthTracker.Application.Features.MoodLogs.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Handlers;

public class GetMoodLogsHandler : IRequestHandler<GetMoodLogsQuery, List<MoodLogDto>>
{
    private readonly IMoodLogRepository _repository;
    public GetMoodLogsHandler(IMoodLogRepository repository) => _repository = repository;

    public async Task<List<MoodLogDto>> Handle(GetMoodLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByUserIdAsync(request.UserId, request.From, request.To);
        return logs.Select(l => new MoodLogDto(l.Id, l.MoodScore, l.Notes, l.LogDate, l.CreatedAt)).ToList();
    }
}