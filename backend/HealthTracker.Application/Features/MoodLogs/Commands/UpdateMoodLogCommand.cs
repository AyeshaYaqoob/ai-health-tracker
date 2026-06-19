using HealthTracker.Application.Features.MoodLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Commands;

public record UpdateMoodLogCommand(
    Guid Id,
    Guid UserId,
    int MoodScore,
    string? Notes
) : IRequest<MoodLogDto>;