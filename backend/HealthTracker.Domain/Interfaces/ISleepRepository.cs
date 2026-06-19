using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface ISleepLogRepository
{
    Task<List<SleepLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to);
    Task<SleepLog?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(SleepLog log);
    void Delete(SleepLog log);
    Task SaveChangesAsync();
}