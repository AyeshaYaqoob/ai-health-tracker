using MediatR;

namespace HealthTracker.Application.Features.SymptomLogs.Commands;

public record DeleteSymptomLogCommand(
    Guid Id,
    Guid UserId
) : IRequest<bool>;