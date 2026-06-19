using HealthTracker.Application.Features.SymptomLogs.DTOs;
using HealthTracker.Application.Features.SymptomLogs.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Handlers;

public class GetSymptomLogsHandler : IRequestHandler<GetSymptomLogsQuery, List<SymptomLogDto>>
{
    private readonly ISymptomLogRepository _repository;

    public GetSymptomLogsHandler(ISymptomLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SymptomLogDto>> Handle(GetSymptomLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByUserIdAsync(request.UserId, request.From, request.To);

        return logs.Select(log => new SymptomLogDto(
            log.Id,
            log.SymptomName,
            log.Severity,
            log.Notes,
            log.LogDate,
            log.CreatedAt
        )).ToList();
    }
}