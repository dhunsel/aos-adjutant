#pragma warning disable MA0048
using System.Diagnostics.CodeAnalysis;

namespace AosAdjutant.Api.Common;

public enum SortDirection
{
    Asc,
    Desc,
}

[SuppressMessage(
    "SonarAnalyzer",
    "S6964",
    Justification = "Because of defaults the issue won't occur."
)]
public abstract record PagedQuery<TSortBy>
    where TSortBy : struct, Enum
{
    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = Math.Max(value, 1);
    }
    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, 100);
    }
    public TSortBy? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}
