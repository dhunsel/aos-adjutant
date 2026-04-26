namespace AosAdjutant.Api.Common;

public record PaginatedResponse<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }

    public PaginatedResponse<TResult> Map<TResult>(Func<T, TResult> map) =>
        new()
        {
            Items = [.. Items.Select(map)],
            TotalCount = TotalCount,
            Page = Page,
            PageSize = PageSize,
            TotalPages = TotalPages,
        };
}
