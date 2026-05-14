using SG.Shared.Enumerators;

namespace SG.Infrastructure.Base.Pagination;

public class PaginationParams
{
    /// <summary>
    /// The page number to retrieve. Default is 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// The number of items to retrieve per page. Default is 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// The field by which to sort the data.
    /// </summary>
    public string? SortField { get; set; }

    /// <summary>
    /// The direction in which to sort the data.
    /// </summary>
    public EnumSortDirection? SortDirection { get; set; } 
    
    /// <summary>
    /// The term to search for in the data.
    /// </summary>
    public string? SearchTerm { get; set; }
}