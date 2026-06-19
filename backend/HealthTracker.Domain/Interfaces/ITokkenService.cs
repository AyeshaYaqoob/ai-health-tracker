using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid GetUserIdFromToken(string accessToken);
}