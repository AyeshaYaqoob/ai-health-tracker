using HealthTracker.Domain.Common;

namespace HealthTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; } = false;

    // Navigation properties (Switched back to singular to match your inner file classes)
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<SymptomLog> SymptomLogs { get; set; } = new List<SymptomLog>();
    public ICollection<MoodLog> MoodLogs { get; set; } = new List<MoodLog>();
    public ICollection<SleepLog> SleepLogs { get; set; } = new List<SleepLog>();
    public ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
    public ICollection<WeeklyReport> WeeklyReports { get; set; } = new List<WeeklyReport>();
}
