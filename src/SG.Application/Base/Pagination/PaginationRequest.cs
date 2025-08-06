using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SG.Infrastructure.Base.Pagination;
using SG.Shared.Enumerators;

namespace SG.Application.Base.Pagination;

public class PaginationRequest
{
    private const int MaxPageSize = 50;

    [Required]
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;

    [Required]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? SearchTerm { get; set; }
    public string? SortField { get; set; }

    public EnumSortDirection SortDirection { get; set; }


    public PaginationParams GetParameters()
        => new PaginationParams
        {
            PageNumber = this.PageNumber,
            PageSize = this.PageSize,
            SortField = this.SortField,
            SortDirection = this.SortDirection,
            SearchTerm = this.SearchTerm
        };
}

