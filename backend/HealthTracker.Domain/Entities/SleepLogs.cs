using HealthTracker.Domain.Common;

namespace HealthTracker.Domain.Entities;

public class SleepLog : BaseEntity
{
    public Guid UserId { get; set; }
    public float HoursSlept { get; set; }
    public int QualityScore { get; set; }   // 1–10
    public TimeOnly BedTime { get; set; }
    public TimeOnly WakeTime { get; set; }
    public DateOnly LogDate { get; set; }

    public User User { get; set; } = null!;
}