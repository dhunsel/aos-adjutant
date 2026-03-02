using AosAdjutant.Database.Configuration;
using AosAdjutant.Features.BattleFormations;
using AosAdjutant.Features.Factions;
using AosAdjutant.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AosAdjutant.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Faction> Factions { get; set; }
    public DbSet<BattleFormation> BattleFormations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new FactionEntityTypeConfiguration().Configure(modelBuilder.Entity<Faction>());
        new BattleFormationEntityTypeConfiguration().Configure(modelBuilder.Entity<BattleFormation>());

        base.OnModelCreating(modelBuilder);
    }
}