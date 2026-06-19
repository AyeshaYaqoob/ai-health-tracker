using HealthTracker.Application.Features.SymptomLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Queries;

public record GetSymptomLogByIdQuery(
    Guid Id,
    Guid UserId
) : IRequest<SymptomLogDto>;