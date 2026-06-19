using HealthTracker.Application.Features.SymptomLogs.Commands;
using HealthTracker.Application.Features.SymptomLogs.DTOs;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Handlers;

public class UpdateSymptomLogHandler : IRequestHandler<UpdateSymptomLogCommand, SymptomLogDto>
{
    private readonly ISymptomLogRepository _repository;

    public UpdateSymptomLogHandler(ISymptomLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<SymptomLogDto> Handle(UpdateSymptomLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Symptom log not found.");

        log.SymptomName = request.SymptomName;
        log.Severity = request.Severity;
        log.Notes = request.Notes;
        log.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

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