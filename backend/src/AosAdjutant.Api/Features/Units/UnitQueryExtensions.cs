using System.Diagnostics;
using System.Linq.Expressions;
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.Units;

public static class UnitQueryExtensions
{
    public static IQueryable<Unit> ApplyFilters(this IQueryable<Unit> query, UnitQuery filter)
    {
        return query;
    }

    public static IQueryable<Unit> ApplySorting(this IQueryable<Unit> query, UnitQuery filter)
    {
        if (filter.SortBy is null)
            return query.OrderBy(u => u.UnitId);

        // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
        // If just using a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
        Expression<Func<Unit, object>> sortExpr = filter.SortBy switch
        {
            UnitSortBy.Name => f => f.Name,
            _ => throw new UnreachableException(),
        };

        return filter.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
