using HealthTracker.Domain.Common;

namespace HealthTracker.Domain.Entities;
public class MoodLog : BaseEntity
{
    public Guid UserId { get; set; }
    public int MoodScore { get; set; }   // 1–10
    public string? Notes { get; set; }
    public DateOnly LogDate { get; set; }

    public User User { get; set; } = null!;
}