using HealthTracker.Application.Features.MoodLogs.Commands;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Handlers;

public class DeleteMoodLogHandler : IRequestHandler<DeleteMoodLogCommand, bool>
{
    private readonly IMoodLogRepository _repository;
    public DeleteMoodLogHandler(IMoodLogRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteMoodLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Mood log not found.");
        _repository.Delete(log);
        await _repository.SaveChangesAsync();
        return true;
    }
}