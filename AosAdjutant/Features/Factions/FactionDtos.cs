using System.ComponentModel.DataAnnotations;

namespace AosAdjutant.Features.Factions;

public record FactionResponseDto
{
    public int FactionId { get; set; }
    public required string Name { get; set; }
    public uint Version { get; set; }
};

public record CreateFactionDto
{
    [StringLength(100, MinimumLength = 1)] public required string Name { get; set; }
};

public record ChangeFactionDto
{
    [StringLength(100, MinimumLength = 1)] public required string Name { get; set; }
    public uint Version { get; set; }
};