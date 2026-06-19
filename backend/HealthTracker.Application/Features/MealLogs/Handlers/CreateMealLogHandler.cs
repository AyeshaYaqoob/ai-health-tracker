using HealthTracker.Application.Features.MealLogs.Commands;
using HealthTracker.Application.Features.MealLogs.DTOs;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Handlers;

public class CreateMealLogHandler : IRequestHandler<CreateMealLogCommand, MealLogDto>
{
    private readonly IMealLogRepository _repository;
    public CreateMealLogHandler(IMealLogRepository repository) => _repository = repository;

    public async Task<MealLogDto> Handle(CreateMealLogCommand request, CancellationToken cancellationToken)
    {
        var validMealTypes = new[] { "Breakfast", "Lunch", "Dinner", "Snack" };
        if (!validMealTypes.Contains(request.MealType))
            throw new ArgumentException("MealType must be Breakfast, Lunch, Dinner or Snack.");

        var log = new MealLog
        {
            UserId = request.UserId,
            MealType = request.MealType,
            Description = request.Description,
            LogDate = request.LogDate
        };

        await _repository.AddAsync(log);
        await _repository.SaveChangesAsync();

        return new MealLogDto(log.Id, log.MealType, log.Description, log.LogDate, log.CreatedAt);
    }
}