using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Infrastructure.Persistence.Repositories;

public class SleepLogRepository : ISleepLogRepository
{
    private readonly AppDbContext _context;
    public SleepLogRepository(AppDbContext context) => _context = context;

    public async Task<List<SleepLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to)
    {
        var query = _context.SleepLogs.Where(s => s.UserId == userId);
        if (from.HasValue) query = query.Where(s => s.LogDate >= from.Value);
        if (to.HasValue) query = query.Where(s => s.LogDate <= to.Value);
        return await query.OrderByDescending(s => s.LogDate).ToListAsync();
    }

    public async Task<SleepLog?> GetByIdAsync(Guid id, Guid userId) =>
        await _context.SleepLogs.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

    public async Task AddAsync(SleepLog log) => await _context.SleepLogs.AddAsync(log);
    public void Delete(SleepLog log) => _context.SleepLogs.Remove(log);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}