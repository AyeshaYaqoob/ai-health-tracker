using HealthTracker.Application.Features.Auth.Commands;
using HealthTracker.Application.Features.Auth.DTOs;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.Auth.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.ToLower())
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddRefreshTokenAsync(refreshToken);
        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto(
            AccessToken: _tokenService.GenerateAccessToken(user),
            RefreshToken: refreshToken.Token,
            ExpiresAt: DateTime.UtcNow.AddMinutes(15),
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email
        );
    }
}