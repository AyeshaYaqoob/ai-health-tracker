using HealthTracker.Domain.Common;

namespace HealthTracker.Domain.Entities;

public class MealLog : BaseEntity
{
    public Guid UserId { get; set; }
    public string MealType { get; set; } = string.Empty;   // Breakfast, Lunch, Dinner, Snack
    public string Description { get; set; } = string.Empty;
    public DateOnly LogDate { get; set; }

    public User User { get; set; } = null!;
}