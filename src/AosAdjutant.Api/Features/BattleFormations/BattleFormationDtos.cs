using System.ComponentModel.DataAnnotations;

namespace AosAdjutant.Api.Features.BattleFormations;

public record BattleFormationResponseDto(int BattleFormationId, string Name, int FactionId, uint Version);

public record CreateBattleFormationDto([StringLength(100, MinimumLength = 1)] string Name);

public record ChangeBattleFormationDto([StringLength(100, MinimumLength = 1)] string Name, uint Version);
