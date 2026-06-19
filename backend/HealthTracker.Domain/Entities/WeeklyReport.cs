using HealthTracker.Domain.Common;

namespace HealthTracker.Domain.Entities;
public class WeeklyReport : BaseEntity
{
    public Guid UserId { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public DateOnly WeekEndDate { get; set; }
    public string AiInsights { get; set; } = string.Empty;
    public string TopSymptoms { get; set; } = string.Empty;
    public float AvgMoodScore { get; set; }
    public float AvgSleepHours { get; set; }
    public bool EmailSent { get; set; } = false;

    public User User { get; set; } = null!;
}