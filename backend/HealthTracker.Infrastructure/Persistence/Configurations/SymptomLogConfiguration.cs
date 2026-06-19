using HealthTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTracker.Infrastructure.Persistence.Configurations;

public class SymptomLogConfiguration : IEntityTypeConfiguration<SymptomLog>
{
    public void Configure(EntityTypeBuilder<SymptomLog> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SymptomName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Severity)
            .IsRequired();

        builder.Property(s => s.Notes)
            .HasMaxLength(500);

        builder.HasIndex(s => new { s.UserId, s.LogDate });
    }
}