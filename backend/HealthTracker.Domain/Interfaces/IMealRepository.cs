using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface IMealLogRepository
{
    Task<List<MealLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to);
    Task<MealLog?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(MealLog log);
    void Delete(MealLog log);
    Task SaveChangesAsync();
}