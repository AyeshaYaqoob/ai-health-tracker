namespace HealthTracker.Application.Features.Auth.DTOs;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    string FirstName,
    string LastName,
    string Email
);