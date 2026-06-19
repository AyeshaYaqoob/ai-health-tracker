using HealthTracker.Application.Features.Auth.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;