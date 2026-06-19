using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Infrastructure.Persistence.Repositories;

public class SymptomLogRepository : ISymptomLogRepository
{
    private readonly AppDbContext _context;

    public SymptomLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SymptomLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to)
    {
        var query = _context.SymptomLogs
            .Where(s => s.UserId == userId);

        if (from.HasValue)
            query = query.Where(s => s.LogDate >= from.Value);

        if (to.HasValue)
            query = query.Where(s => s.LogDate <= to.Value);

        return await query
            .OrderByDescending(s => s.LogDate)
            .ToListAsync();
    }

    public async Task<SymptomLog?> GetByIdAsync(Guid id, Guid userId) =>
        await _context.SymptomLogs
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

    public async Task AddAsync(SymptomLog log) =>
        await _context.SymptomLogs.AddAsync(log);

    public void Delete(SymptomLog log) =>
        _context.SymptomLogs.Remove(log);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}