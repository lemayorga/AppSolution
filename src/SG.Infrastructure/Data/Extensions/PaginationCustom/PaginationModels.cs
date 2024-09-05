namespace SG.Infrastructure.Data.Extensions.PaginationCustom.PaginationModels;

public class ColumnFilter
{
    public required string ColumnName { get; set; } 
    public required string Value { get; set; }
}

public class ColumnSorting
{
    public required string ColumnName { get; set; }
    public required bool Desc { get; set; }
}
