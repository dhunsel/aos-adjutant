using AosAdjutant.Features.Factions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AosAdjutant.Database.Configuration;

public class FactionEntityTypeConfiguration : IEntityTypeConfiguration<Faction>
{
    public void Configure(EntityTypeBuilder<Faction> builder)
    {
        builder.ToTable("faction");
        builder.Property(f => f.FactionId).HasColumnName("faction_id");
        builder.Property(f => f.Name).HasColumnName("name").HasMaxLength(250);

        builder.HasKey(f => f.FactionId);

        builder.HasIndex(f => f.Name).IsUnique();

        builder
            .HasMany(f => f.BattleFormations)
            .WithOne(bf => bf.Faction)
            .HasForeignKey(bf => bf.FactionId)
            .IsRequired();

        builder.Property(f => f.Version).IsRowVersion();
    }
}