using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(User user);
    Task SaveChangesAsync();
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
}