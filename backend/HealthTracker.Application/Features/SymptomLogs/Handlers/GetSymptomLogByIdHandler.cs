using HealthTracker.Application.Features.SymptomLogs.DTOs;
using HealthTracker.Application.Features.SymptomLogs.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Handlers;

public class GetSymptomLogByIdHandler : IRequestHandler<GetSymptomLogByIdQuery, SymptomLogDto>
{
    private readonly ISymptomLogRepository _repository;

    public GetSymptomLogByIdHandler(ISymptomLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<SymptomLogDto> Handle(GetSymptomLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Symptom log not found.");

        return new SymptomLogDto(
            log.Id,
            log.SymptomName,
            log.Severity,
            log.Notes,
            log.LogDate,
            log.CreatedAt
        );
    }
}