using HealthTracker.Application.Features.MoodLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Commands;

public record CreateMoodLogCommand(
    Guid UserId,
    int MoodScore,
    string? Notes,
    DateOnly LogDate
) : IRequest<MoodLogDto>;