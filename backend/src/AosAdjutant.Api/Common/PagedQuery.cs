namespace AosAdjutant.Api.Common;

public abstract record PagedQuery
{
    public required int Page { get; set; } = 1;
    private int _pageSize = 20;
    public required int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, 100);
    }
    public string? SortBy { get; set; }
    public required string SortDirection { get; set; } = "asc";
}
