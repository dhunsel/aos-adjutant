namespace AosAdjutant.Api.Common;

public record PaginatedResponse<T>
{
    public required IReadOnlyList<T> Items { get; init; } = [];
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPages { get; init; }

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
