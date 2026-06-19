using HealthTracker.Application.Features.Auth.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.Auth.Commands;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<AuthResponseDto>;