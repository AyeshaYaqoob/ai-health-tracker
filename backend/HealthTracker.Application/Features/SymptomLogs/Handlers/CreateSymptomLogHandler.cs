using HealthTracker.Application.Features.SymptomLogs.Commands;
using HealthTracker.Application.Features.SymptomLogs.DTOs;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Handlers;

public class CreateSymptomLogHandler : IRequestHandler<CreateSymptomLogCommand, SymptomLogDto>
{
    private readonly ISymptomLogRepository _repository;

    public CreateSymptomLogHandler(ISymptomLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<SymptomLogDto> Handle(CreateSymptomLogCommand request, CancellationToken cancellationToken)
    {
        if (request.Severity < 1 || request.Severity > 10)
            throw new ArgumentException("Severity must be between 1 and 10.");

        var log = new SymptomLog
        {
            UserId = request.UserId,
            SymptomName = request.SymptomName,
            Severity = request.Severity,
            Notes = request.Notes,
            LogDate = request.LogDate
        };

        await _repository.AddAsync(log);
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