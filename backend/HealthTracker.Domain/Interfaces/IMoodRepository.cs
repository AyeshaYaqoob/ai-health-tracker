using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface IMoodLogRepository
{
    Task<List<MoodLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to);
    Task<MoodLog?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(MoodLog log);
    void Delete(MoodLog log);
    Task SaveChangesAsync();
}