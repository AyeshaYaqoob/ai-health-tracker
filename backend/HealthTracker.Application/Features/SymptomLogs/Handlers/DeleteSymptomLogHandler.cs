using HealthTracker.Application.Features.SymptomLogs.Commands;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Handlers;

public class DeleteSymptomLogHandler : IRequestHandler<DeleteSymptomLogCommand, bool>
{
    private readonly ISymptomLogRepository _repository;

    public DeleteSymptomLogHandler(ISymptomLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteSymptomLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Symptom log not found.");

        _repository.Delete(log);
        await _repository.SaveChangesAsync();

        return true;
    }
}