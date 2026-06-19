using HealthTracker.Application.Features.MealLogs.Commands;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Handlers;

public class DeleteMealLogHandler : IRequestHandler<DeleteMealLogCommand, bool>
{
    private readonly IMealLogRepository _repository;
    public DeleteMealLogHandler(IMealLogRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteMealLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Meal log not found.");
        _repository.Delete(log);
        await _repository.SaveChangesAsync();
        return true;
    }
}