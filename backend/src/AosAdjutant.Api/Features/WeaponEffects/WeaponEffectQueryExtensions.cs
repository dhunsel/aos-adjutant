using System.Diagnostics;
using System.Linq.Expressions;
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.WeaponEffects;

public static class WeaponEffectQueryExtensions
{
    public static IQueryable<WeaponEffect> ApplyFilters(
        this IQueryable<WeaponEffect> query,
        WeaponEffectQuery filter
    )
    {
        return query;
    }

    public static IQueryable<WeaponEffect> ApplySorting(
        this IQueryable<WeaponEffect> query,
        WeaponEffectQuery filter
    )
    {
        if (filter.SortBy is null)
            return query.OrderBy(we => we.Key);

        // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
        // If just using a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
        Expression<Func<WeaponEffect, object>> sortExpr = filter.SortBy switch
        {
            WeaponEffectSortBy.Name => f => f.Name,
            _ => throw new UnreachableException(),
        };

        return filter.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
