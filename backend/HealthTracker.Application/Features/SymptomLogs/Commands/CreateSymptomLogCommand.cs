using HealthTracker.Application.Features.SymptomLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Commands;

public record CreateSymptomLogCommand(
    Guid UserId,
    string SymptomName,
    int Severity,
    string? Notes,
    DateOnly LogDate
) : IRequest<SymptomLogDto>;