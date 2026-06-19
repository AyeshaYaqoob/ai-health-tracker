using HealthTracker.Application.Features.SymptomLogs.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Commands;

public record UpdateSymptomLogCommand(
    Guid Id,
    Guid UserId,
    string SymptomName,
    int Severity,
    string? Notes
) : IRequest<SymptomLogDto>;