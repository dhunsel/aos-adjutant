using System.ComponentModel.DataAnnotations;

namespace AosAdjutant.Api.Features.Factions;

public record FactionResponseDto(int FactionId, string Name, uint Version);

public record CreateFactionDto([StringLength(100, MinimumLength = 1)] string Name);

public record ChangeFactionDto([StringLength(100, MinimumLength = 1)] string Name, uint Version);