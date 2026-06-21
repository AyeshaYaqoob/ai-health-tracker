using FluentAssertions;
using HealthTracker.Application.Features.Auth.Commands;
using HealthTracker.Application.Features.Auth.DTOs;
using HealthTracker.Application.Features.Auth.Handlers;
using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Moq;

namespace HealthTracker.Tests.Unit.Handlers;

public class RegisterHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly RegisterHandler _handler;

    public RegisterHandlerTests()
    {
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("refresh-token-abc");
        _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>())).Returns("access-token-xyz");
        _handler = new RegisterHandler(_userRepoMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidNewUser_ReturnsAuthResponse()
    {
        // Arrange
        _userRepoMock.Setup(r => r.EmailExistsAsync("test@example.com")).ReturnsAsync(false);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var command = new RegisterCommand("Test", "User", "test@example.com", "Password123!");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token-xyz");
        result.RefreshToken.Should().Be("refresh-token-abc");
        result.FirstName.Should().Be("Test");
        result.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        _userRepoMock.Setup(r => r.EmailExistsAsync("existing@example.com")).ReturnsAsync(true);
        var command = new RegisterCommand("Test", "User", "existing@example.com", "Password123!");

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email already registered.");
    }

    [Fact]
    public async Task Handle_ValidUser_SavesWithBcryptHash()
    {
        // Arrange
        User? savedUser = null;
        _userRepoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => savedUser = u)
            .Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var command = new RegisterCommand("Ayesha", "Yaqoob", "ayesha@test.com", "MyPassword99!");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedUser.Should().NotBeNull();
        savedUser!.PasswordHash.Should().NotBe("MyPassword99!");  // Must be hashed
        BCrypt.Net.BCrypt.Verify("MyPassword99!", savedUser.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ValidUser_NormalizesEmailToLowercase()
    {
        // Arrange
        User? savedUser = null;
        _userRepoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => savedUser = u)
            .Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var command = new RegisterCommand("Test", "User", "TEST@EXAMPLE.COM", "Password123!");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedUser!.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task Handle_ValidUser_CallsSaveChangesExactlyOnce()
    {
        // Arrange
        _userRepoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var command = new RegisterCommand("Test", "User", "test@test.com", "Password123!");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
