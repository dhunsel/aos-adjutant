using System.Linq.Expressions;
using AosAdjutant.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Features.Factions;

public static class FactionQueryExtensions
{
    // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
    // If just returning a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
    private static readonly Dictionary<string, Expression<Func<Faction, object>>> SortColumns = new(
        StringComparer.OrdinalIgnoreCase
    )
    {
        ["name"] = f => f.Name,
        ["grandAlliance"] = f => f.GrandAlliance,
    };

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
        PagedQuery filter
    )
    {
        // Row order without explicit order by is undefined, therefore always fall back on id sorting
        if (filter.SortBy is null || !SortColumns.TryGetValue(filter.SortBy, out var sortExpr))
            return query.OrderBy(f => f.FactionId);

        return filter.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
