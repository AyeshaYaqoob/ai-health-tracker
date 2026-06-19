using Microsoft.EntityFrameworkCore;
using HealthTracker.Domain.Entities;
using System.Reflection;

namespace HealthTracker.Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>(); 
    public DbSet<SymptomLog> SymptomLogs => Set<SymptomLog>();
    public DbSet<MoodLog> MoodLogs => Set<MoodLog>();
    public DbSet<MealLog> MealLogs => Set<MealLog>();
    public DbSet<SleepLog> SleepLogs => Set<SleepLog>();
    public DbSet<WeeklyReport> WeeklyReports => Set<WeeklyReport>();

    protected override void OnModelCreating(ModelBuilder builder){
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}