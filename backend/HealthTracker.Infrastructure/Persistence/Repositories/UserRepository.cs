using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id);
    public async Task<List<User>> GetAllUsersAsync() =>
    await _context.Users.ToListAsync();

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
    public async Task AddRefreshTokenAsync(RefreshToken refreshToken) =>
    await _context.RefreshTokens.AddAsync(refreshToken);
}