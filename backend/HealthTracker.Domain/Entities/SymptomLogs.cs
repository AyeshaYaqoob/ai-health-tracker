using HealthTracker.Domain.Common;

namespace HealthTracker.Domain.Entities;

public class SymptomLog : BaseEntity
{
    public Guid UserId {get ; set;}
    public string SymptomName {get; set;} = string.Empty;
    public int Severity {get; set;}  //1-10
    public string? Notes{get; set;}
    public DateOnly LogDate {get; set;}
    public User User {get; set;} = null!; //navigation property
}