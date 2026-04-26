using System.Diagnostics;
using System.Linq.Expressions;
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.BattleFormations;

public static class BattleFormationQueryExtensions
{
    public static IQueryable<BattleFormation> ApplyFilters(
        this IQueryable<BattleFormation> query,
        BattleFormationQuery filter
    )
    {
        return query;
    }

    public static IQueryable<BattleFormation> ApplySorting(
        this IQueryable<BattleFormation> query,
        BattleFormationQuery filter
    )
    {
        if (filter.SortBy is null)
            return query.OrderBy(bf => bf.BattleFormationId);

        // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
        // If just using a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
        Expression<Func<BattleFormation, object>> sortExpr = filter.SortBy switch
        {
            BattleFormationSortBy.Name => bf => bf.Name,
            _ => throw new UnreachableException(),
        };

        return filter.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
