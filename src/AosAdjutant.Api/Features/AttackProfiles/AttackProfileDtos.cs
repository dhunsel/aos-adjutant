using System.ComponentModel.DataAnnotations;
using AosAdjutant.Api.Features.WeaponEffects;

namespace AosAdjutant.Api.Features.AttackProfiles;

public sealed record AttackProfileResponseDto(
    int AttackProfileId,
    string Name,
    bool IsRanged,
    int? Range,
    string Attacks,
    int ToHit,
    int ToWound,
    int? Rend,
    string Damage,
    int UnitId,
    uint Version,
    List<WeaponEffectResponseDto> WeaponEffects
);

public sealed record CreateAttackProfileDto(
    [StringLength(100, MinimumLength = 1)] string Name,
    bool IsRanged,
    int? Range,
    [StringLength(100, MinimumLength = 1)] string Attacks,
    int ToHit,
    int ToWound,
    int? Rend,
    [StringLength(100, MinimumLength = 1)] string Damage,
    List<string> WeaponEffects
);

public sealed record ChangeAttackProfileDto(
    [StringLength(100, MinimumLength = 1)] string Name,
    bool IsRanged,
    int? Range,
    [StringLength(100, MinimumLength = 1)] string Attacks,
    int ToHit,
    int ToWound,
    int? Rend,
    [StringLength(100, MinimumLength = 1)] string Damage,
    uint Version,
    List<string> WeaponEffects
);
