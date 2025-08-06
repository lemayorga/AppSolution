using SG.Shared.Enumerators;

namespace SG.Infrastructure.Base.Pagination;

public class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortField { get; set; }
    public EnumSortDirection? SortDirection { get; set; } 
    public string? SearchTerm { get; set; }
}