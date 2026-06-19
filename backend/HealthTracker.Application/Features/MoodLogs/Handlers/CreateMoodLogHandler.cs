using HealthTracker.Application.Features.MoodLogs.Commands;
using HealthTracker.Application.Features.MoodLogs.DTOs;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Handlers;

public class CreateMoodLogHandler : IRequestHandler<CreateMoodLogCommand, MoodLogDto>
{
    private readonly IMoodLogRepository _repository;
    public CreateMoodLogHandler(IMoodLogRepository repository) => _repository = repository;

    public async Task<MoodLogDto> Handle(CreateMoodLogCommand request, CancellationToken cancellationToken)
    {
        if (request.MoodScore < 1 || request.MoodScore > 10)
            throw new ArgumentException("Mood score must be between 1 and 10.");

        var log = new MoodLog
        {
            UserId = request.UserId,
            MoodScore = request.MoodScore,
            Notes = request.Notes,
            LogDate = request.LogDate
        };

        await _repository.AddAsync(log);
        await _repository.SaveChangesAsync();

        return new MoodLogDto(log.Id, log.MoodScore, log.Notes, log.LogDate, log.CreatedAt);
    }
}