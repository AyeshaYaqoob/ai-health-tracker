using HealthTracker.Application.Features.SleepLogs.Commands;
using HealthTracker.Application.Features.SleepLogs.DTOs;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Handlers;

public class UpdateSleepLogHandler : IRequestHandler<UpdateSleepLogCommand, SleepLogDto>
{
    private readonly ISleepLogRepository _repository;
    public UpdateSleepLogHandler(ISleepLogRepository repository) => _repository = repository;

    public async Task<SleepLogDto> Handle(UpdateSleepLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Sleep log not found.");

        log.HoursSlept = request.HoursSlept;
        log.QualityScore = request.QualityScore;
        log.BedTime = request.BedTime;
        log.WakeTime = request.WakeTime;
        log.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return new SleepLogDto(log.Id, log.HoursSlept, log.QualityScore, log.BedTime, log.WakeTime, log.LogDate, log.CreatedAt);
    }
}