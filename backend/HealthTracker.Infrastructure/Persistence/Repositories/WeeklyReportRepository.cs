using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Infrastructure.Persistence.Repositories;

public class WeeklyReportRepository : IWeeklyReportRepository
{
    private readonly AppDbContext _context;
    public WeeklyReportRepository(AppDbContext context) => _context = context;

    public async Task<List<WeeklyReport>> GetByUserIdAsync(Guid userId) =>
        await _context.WeeklyReports
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.WeekStartDate)
            .ToListAsync();

    public async Task<WeeklyReport?> GetByIdAsync(Guid id, Guid userId) =>
        await _context.WeeklyReports
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

    public async Task<bool> ReportExistsAsync(Guid userId, DateOnly weekStart) =>
        await _context.WeeklyReports
            .AnyAsync(r => r.UserId == userId && r.WeekStartDate == weekStart);

    public async Task AddAsync(WeeklyReport report) =>
        await _context.WeeklyReports.AddAsync(report);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}