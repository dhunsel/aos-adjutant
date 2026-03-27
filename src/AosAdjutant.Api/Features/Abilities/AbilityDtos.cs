using System.ComponentModel.DataAnnotations;

namespace AosAdjutant.Api.Features.Abilities;

public sealed record AbilityResponseDto(
    int AbilityId,
    string Name,
    string? Reaction,
    string? Declaration,
    string Effect,
    TurnPhase Phase,
    ActivationRestriction? Restriction,
    PlayerTurn? Turn,
    uint Version
);

public sealed record CreateAbilityDto(
    [StringLength(100, MinimumLength = 1)] string Name,
    [StringLength(100, MinimumLength = 1)] string? Reaction,
    [StringLength(100, MinimumLength = 1)] string? Declaration,
    [StringLength(100, MinimumLength = 1)] string Effect,
    TurnPhase Phase,
    ActivationRestriction? Restriction,
    PlayerTurn? Turn
);

public sealed record ChangeAbilityDto(
    [StringLength(100, MinimumLength = 1)] string Name,
    [StringLength(100, MinimumLength = 1)] string? Reaction,
    [StringLength(100, MinimumLength = 1)] string? Declaration,
    [StringLength(100, MinimumLength = 1)] string Effect,
    TurnPhase Phase,
    ActivationRestriction? Restriction,
    PlayerTurn? Turn,
    uint Version
);
