using HealthTracker.Application.Features.Auth.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.Auth.Commands;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<AuthResponseDto>;