using System.Diagnostics;
using System.Linq.Expressions;
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.Factions;

public static class FactionQueryExtensions
{
    public static IQueryable<Faction> ApplyFilters(
        this IQueryable<Faction> query,
        FactionQuery filter
    )
    {
        if (filter.GrandAlliance is not null)
            query = query.Where(f => f.GrandAlliance == filter.GrandAlliance);

        return query;
    }

    public static IQueryable<Faction> ApplySorting(
        this IQueryable<Faction> query,
        FactionQuery filter
    )
    {
        if (filter.SortBy is null)
            return query.OrderBy(f => f.FactionId);

        // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
        // If just using a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
        Expression<Func<Faction, object>> sortExpr = filter.SortBy switch
        {
            FactionSortBy.Name => f => f.Name,
            FactionSortBy.GrandAlliance => f => f.GrandAlliance,
            _ => throw new UnreachableException(),
        };

        return filter.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
