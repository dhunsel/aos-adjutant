using System.ComponentModel.DataAnnotations;

namespace AosAdjutant.Api.Features.Factions;

public sealed record FactionResponseDto(int FactionId, string Name, uint Version);

public sealed record CreateFactionDto([StringLength(100, MinimumLength = 1)] string Name);

public sealed record ChangeFactionDto([StringLength(100, MinimumLength = 1)] string Name, uint Version);
