namespace SG.Domain;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 6;
    public int PageSize { get { return _pageSize; } set { _pageSize = value > MaxPageSize ? MaxPageSize : value; } }
}

public class SearchParams: PaginationParams
{        
    public string? OrderBy { get; set; }
    public string? SearchTerm { get; set; }
    public string? ColumnFilters { get; set; }
}
