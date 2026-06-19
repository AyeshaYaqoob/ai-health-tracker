using HealthTracker.Application.Features.MealLogs.Commands;
using HealthTracker.Application.Features.MealLogs.DTOs;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Handlers;

public class UpdateMealLogHandler : IRequestHandler<UpdateMealLogCommand, MealLogDto>
{
    private readonly IMealLogRepository _repository;
    public UpdateMealLogHandler(IMealLogRepository repository) => _repository = repository;

    public async Task<MealLogDto> Handle(UpdateMealLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Meal log not found.");

        log.MealType = request.MealType;
        log.Description = request.Description;
        log.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return new MealLogDto(log.Id, log.MealType, log.Description, log.LogDate, log.CreatedAt);
    }
}