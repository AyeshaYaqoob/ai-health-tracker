using HealthTracker.Application.Features.MealLogs.DTOs;
using HealthTracker.Application.Features.MealLogs.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MealLogs.Handlers;

public class GetMealLogsHandler : IRequestHandler<GetMealLogsQuery, List<MealLogDto>>
{
    private readonly IMealLogRepository _repository;
    public GetMealLogsHandler(IMealLogRepository repository) => _repository = repository;

    public async Task<List<MealLogDto>> Handle(GetMealLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByUserIdAsync(request.UserId, request.From, request.To);
        return logs.Select(l => new MealLogDto(l.Id, l.MealType, l.Description, l.LogDate, l.CreatedAt)).ToList();
    }
}