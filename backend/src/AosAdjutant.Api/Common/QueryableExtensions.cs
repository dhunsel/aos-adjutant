using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Common;

public static class QueryableExtensions
{
    public static async Task<PaginatedResponse<T>> ToPaginatedReponse<T, TSortBy>(
        this IQueryable<T> query,
        PagedQuery<TSortBy> filter
    )
        where TSortBy : struct, Enum
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginatedResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize),
        };
    }
}
