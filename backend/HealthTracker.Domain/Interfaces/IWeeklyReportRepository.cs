using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface IWeeklyReportRepository
{
    Task<List<WeeklyReport>> GetByUserIdAsync(Guid userId);
    Task<WeeklyReport?> GetByIdAsync(Guid id, Guid userId);
    Task<bool> ReportExistsAsync(Guid userId, DateOnly weekStart);
    Task AddAsync(WeeklyReport report);
    Task SaveChangesAsync();
}