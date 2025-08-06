
using System.ComponentModel.DataAnnotations;

namespace SG.Infrastructure.Base.Pagination;

public class FilterColumn
{
    public string Field { get; set; } = default!;
    
    [RegularExpression("^(eq|neq|contains|gt|lt)$")]
    public string Operator { get; set; } = default!;
    public string Value { get; set; }  = default!;
}