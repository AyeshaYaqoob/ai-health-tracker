using FluentAssertions;
using HealthTracker.Application.Features.Auth.Commands;
using HealthTracker.Application.Features.Auth.Handlers;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Moq;

namespace HealthTracker.Tests.Unit.Handlers;

public class LoginHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("refresh-token");
        _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>())).Returns("access-token");
        _handler = new LoginHandler(_userRepoMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_CorrectCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = passwordHash
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync("user@example.com")).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.AddRefreshTokenAsync(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new LoginCommand("user@example.com", "Password123!"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.FirstName.Should().Be("Test");
        result.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task Handle_WrongPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var user = new User
        {
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!")
        };
        _userRepoMock.Setup(r => r.GetByEmailAsync("user@example.com")).ReturnsAsync(user);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(new LoginCommand("user@example.com", "WrongPassword!"), CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password.");
    }

    [Fact]
    public async Task Handle_NonExistentEmail_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByEmailAsync("noone@example.com")).ReturnsAsync((User?)null);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(new LoginCommand("noone@example.com", "anything"), CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password.");
    }

    [Fact]
    public async Task Handle_SuccessfulLogin_CreatesAndSavesRefreshToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            FirstName = "Test", LastName = "User"
        };
        RefreshToken? savedToken = null;
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.AddRefreshTokenAsync(It.IsAny<RefreshToken>()))
            .Callback<RefreshToken>(t => savedToken = t)
            .Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(new LoginCommand("user@example.com", "Password123!"), CancellationToken.None);

        // Assert
        savedToken.Should().NotBeNull();
        savedToken!.UserId.Should().Be(user.Id);
        savedToken.ExpiresAt.Should().BeAfter(DateTime.UtcNow.AddDays(6));
    }
}
