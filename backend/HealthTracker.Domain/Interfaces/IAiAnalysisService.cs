namespace HealthTracker.Domain.Interfaces;

public interface IAiAnalysisService
{
    Task<string> AnalyzeHealthLogsAsync(HealthDataContext context);
}

public class HealthDataContext
{
    public Guid UserId { get; set; }
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public List<string> Symptoms { get; set; } = new();
    public List<string> Moods { get; set; } = new();
    public List<string> Sleep { get; set; } = new();
    public List<string> Meals { get; set; } = new();
}