namespace AosAdjutant.Features.Factions;

public static class FactionMappings
{
    public static FactionResponseDto ToResponseDto(this Faction faction) =>
        new() { FactionId = faction.FactionId, Name = faction.Name, Version = faction.Version };
}