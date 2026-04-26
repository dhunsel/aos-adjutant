using System.Diagnostics;
using System.Linq.Expressions;
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.Abilities;

public static class AbilityQueryExtensions
{
    public static IQueryable<Ability> ApplyFilters(
        this IQueryable<Ability> query,
        AbilityQuery filter
    )
    {
        if (filter.Phase is not null)
            query = query.Where(a => a.Phase == filter.Phase);

        return query;
    }

    public static IQueryable<Ability> ApplySorting(
        this IQueryable<Ability> query,
        AbilityQuery filter
    )
    {
        if (filter.SortBy is null)
            return query.OrderBy(a => a.AbilityId);

        // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
        // If just using a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
        Expression<Func<Ability, object>> sortExpr = filter.SortBy switch
        {
            AbilitySortBy.Name => a => a.Name,
            AbilitySortBy.Phase => a => a.Phase,
            _ => throw new UnreachableException(),
        };

        return filter.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
