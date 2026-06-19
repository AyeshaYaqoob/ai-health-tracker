using HealthTracker.Domain.Entities;

namespace HealthTracker.Domain.Interfaces;

public interface ISymptomLogRepository
{
    Task<List<SymptomLog>> GetByUserIdAsync(Guid userId, DateOnly? from, DateOnly? to);
    Task<SymptomLog?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(SymptomLog log);
    void Delete(SymptomLog log);
    Task SaveChangesAsync();
}