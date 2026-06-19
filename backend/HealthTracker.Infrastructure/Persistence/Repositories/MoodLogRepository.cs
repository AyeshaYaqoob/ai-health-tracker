using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Infrastructure.Persistence.Repositories;

public class MoodLogRepository : IMoodLogRepository
{
    private readonly AppDbContext _context;
    public MoodLogRepository(AppDbContext context) => _context = context;

    public async Task<List<MoodLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to)
    {
        var query = _context.MoodLogs.Where(m => m.UserId == userId);
        if (from.HasValue) query = query.Where(m => m.LogDate >= from.Value);
        if (to.HasValue) query = query.Where(m => m.LogDate <= to.Value);
        return await query.OrderByDescending(m => m.LogDate).ToListAsync();
    }

    public async Task<MoodLog?> GetByIdAsync(Guid id, Guid userId) =>
        await _context.MoodLogs.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

    public async Task AddAsync(MoodLog log) => await _context.MoodLogs.AddAsync(log);
    public void Delete(MoodLog log) => _context.MoodLogs.Remove(log);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}