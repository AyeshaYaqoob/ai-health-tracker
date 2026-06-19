using HealthTracker.Application.Features.SymptomLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Queries;

public record GetSymptomLogsQuery(
    Guid UserId,
    DateOnly? From,
    DateOnly? To
) : IRequest<List<SymptomLogDto>>;