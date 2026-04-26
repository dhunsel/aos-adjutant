using System.Diagnostics;
using System.Linq.Expressions;
using AosAdjutant.Api.Common;

namespace AosAdjutant.Api.Features.AttackProfiles;

public static class AttackProfileQueryExtensions
{
    public static IQueryable<AttackProfile> ApplyFilters(
        this IQueryable<AttackProfile> query,
        AttackProfileQuery filter
    )
    {
        if (filter.IsRanged is not null)
            query = query.Where(ap => ap.IsRanged == filter.IsRanged);

        return query;
    }

    public static IQueryable<AttackProfile> ApplySorting(
        this IQueryable<AttackProfile> query,
        AttackProfileQuery filter
    )
    {
        if (filter.SortBy is null)
            return query.OrderBy(ap => ap.AttackProfileId);

        // Keep the sorting argument stored as Expression so that the OrderBy overload of IQueryable is used
        // If just using a delegate, the IEnumerable OrderBy would be used which would sort the results in C#, not in DB
        Expression<Func<AttackProfile, object>> sortExpr = filter.SortBy switch
        {
            AttackProfileSortBy.Name => ap => ap.Name,
            AttackProfileSortBy.IsRanged => ap => ap.IsRanged,
            _ => throw new UnreachableException(),
        };

        return filter.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);
    }
}
