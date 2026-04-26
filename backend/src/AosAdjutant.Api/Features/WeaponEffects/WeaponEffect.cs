#pragma warning disable MA0048
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.WeaponEffects;

public sealed class WeaponEffect
{
    public int WeaponEffectId { get; set; }
    public required string Key { get; set; }
    public required string Name { get; set; }
}

public enum WeaponEffectSortBy
{
    Name,
}

public sealed record WeaponEffectQuery : PagedQuery<WeaponEffectSortBy>;
