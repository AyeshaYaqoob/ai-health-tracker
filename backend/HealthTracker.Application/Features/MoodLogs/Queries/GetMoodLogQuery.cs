using HealthTracker.Application.Features.MoodLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.MoodLogs.Queries;

public record GetMoodLogsQuery(
    Guid UserId,
    DateOnly? From,
    DateOnly? To
) : IRequest<List<MoodLogDto>>;