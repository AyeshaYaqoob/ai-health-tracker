using HealthTracker.Application.Features.SleepLogs.Commands;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Handlers;

public class DeleteSleepLogHandler : IRequestHandler<DeleteSleepLogCommand, bool>
{
    private readonly ISleepLogRepository _repository;
    public DeleteSleepLogHandler(ISleepLogRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteSleepLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Sleep log not found.");
        _repository.Delete(log);
        await _repository.SaveChangesAsync();
        return true;
    }
}