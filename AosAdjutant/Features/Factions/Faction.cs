using AosAdjutant.Features.BattleFormations;

namespace AosAdjutant.Features.Factions;

public class Faction
{
    public int FactionId { get; set; }
    public required string Name { get; set; }
    public uint Version { get; set; }

    public ICollection<BattleFormation> BattleFormations { get; } = new List<BattleFormation>();
}