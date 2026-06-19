using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Infrastructure.Persistence.Repositories;

public class MealLogRepository : IMealLogRepository
{
    private readonly AppDbContext _context;
    public MealLogRepository(AppDbContext context) => _context = context;

    public async Task<List<MealLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to)
    {
        var query = _context.MealLogs.Where(m => m.UserId == userId);
        if (from.HasValue) query = query.Where(m => m.LogDate >= from.Value);
        if (to.HasValue) query = query.Where(m => m.LogDate <= to.Value);
        return await query.OrderByDescending(m => m.LogDate).ToListAsync();
    }

    public async Task<MealLog?> GetByIdAsync(Guid id, Guid userId) =>
        await _context.MealLogs.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

    public async Task AddAsync(MealLog log) => await _context.MealLogs.AddAsync(log);
    public void Delete(MealLog log) => _context.MealLogs.Remove(log);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}