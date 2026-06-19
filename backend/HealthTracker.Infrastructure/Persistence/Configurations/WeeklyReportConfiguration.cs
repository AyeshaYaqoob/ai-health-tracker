using HealthTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTracker.Infrastructure.Persistence.Configurations;

public class WeeklyReportConfiguration : IEntityTypeConfiguration<WeeklyReport>
{
    public void Configure(EntityTypeBuilder<WeeklyReport> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.AiInsights)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(w => w.TopSymptoms)
            .HasMaxLength(500);

        builder.HasIndex(w => new { w.UserId, w.WeekStartDate })
            .IsUnique();
    }
}