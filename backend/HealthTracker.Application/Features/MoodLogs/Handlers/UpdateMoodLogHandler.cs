using HealthTracker.Application.Features.MoodLogs.Commands;
using HealthTracker.Application.Features.MoodLogs.DTOs;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Handlers;

public class UpdateMoodLogHandler : IRequestHandler<UpdateMoodLogCommand, MoodLogDto>
{
    private readonly IMoodLogRepository _repository;
    public UpdateMoodLogHandler(IMoodLogRepository repository) => _repository = repository;

    public async Task<MoodLogDto> Handle(UpdateMoodLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.Id, request.UserId)
            ?? throw new KeyNotFoundException("Mood log not found.");

        log.MoodScore = request.MoodScore;
        log.Notes = request.Notes;
        log.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return new MoodLogDto(log.Id, log.MoodScore, log.Notes, log.LogDate, log.CreatedAt);
    }
}