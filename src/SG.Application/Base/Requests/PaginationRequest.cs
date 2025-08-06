// namespace SG.Application.Base.Requests;

// public class PaginationRequest
// {
//     private const int MaxPageSize = 50;
//     public int PageNumber { get; set; } = 1;
//     private int _pageSize = 10;

//     public int PageSize
//     {
//         get => _pageSize;
//         set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
//     }
//     public string? SearchTerm { get; set; }
// }

// public class FilterColumn
// {
//     public string Field { get; set; } = default!;
//     public string Operator { get; set; }  = default!;
//     public string Value { get; set; }  = default!;
// }