using AosAdjutant.Features.BattleFormations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AosAdjutant.Database.Configuration;

public class BattleFormationEntityTypeConfiguration : IEntityTypeConfiguration<BattleFormation>
{
    public void Configure(EntityTypeBuilder<BattleFormation> builder)
    {
        builder.ToTable("battle_formation");
        builder.Property(bf => bf.BattleFormationId).HasColumnName("battle_formation_id");
        builder.Property(bf => bf.Name).HasColumnName("name").HasMaxLength(250);
        builder.Property(bf => bf.FactionId).HasColumnName("faction_id");

        builder.HasKey(bf => bf.BattleFormationId);

        builder.HasIndex(bf => bf.Name).IsUnique();

        builder.Property(bf => bf.Version).IsRowVersion();
    }
}