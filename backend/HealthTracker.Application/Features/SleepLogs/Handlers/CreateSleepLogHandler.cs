using HealthTracker.Application.Features.SleepLogs.Commands;
using HealthTracker.Application.Features.SleepLogs.DTOs;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.SleepLogs.Handlers;

public class CreateSleepLogHandler : IRequestHandler<CreateSleepLogCommand, SleepLogDto>
{
    private readonly ISleepLogRepository _repository;
    public CreateSleepLogHandler(ISleepLogRepository repository) => _repository = repository;

    public async Task<SleepLogDto> Handle(CreateSleepLogCommand request, CancellationToken cancellationToken)
    {
        if (request.QualityScore < 1 || request.QualityScore > 10)
            throw new ArgumentException("Quality score must be between 1 and 10.");

        var log = new SleepLog
        {
            UserId = request.UserId,
            HoursSlept = request.HoursSlept,
            QualityScore = request.QualityScore,
            BedTime = request.BedTime,
            WakeTime = request.WakeTime,
            LogDate = request.LogDate
        };

        await _repository.AddAsync(log);
        await _repository.SaveChangesAsync();

        return new SleepLogDto(log.Id, log.HoursSlept, log.QualityScore, log.BedTime, log.WakeTime, log.LogDate, log.CreatedAt);
    }
}