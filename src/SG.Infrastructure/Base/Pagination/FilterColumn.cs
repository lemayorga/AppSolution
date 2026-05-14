
using System.ComponentModel.DataAnnotations;

namespace SG.Infrastructure.Base.Pagination;

public class FilterColumn
{
    /// <summary>
    /// Name of the field by which the data will be filtered
    /// </summary>
    public string Field { get; set; } = default!;
    
    /// <summary>
    /// Operator by which the data will be filtered. Possible values: eq, neq, contains, gt, lt
    /// </summary>
    [RegularExpression("^(eq|neq|contains|gt|lt)$")]
    public string Operator { get; set; } = default!;
    
    /// <summary>
    /// Value by which the data will be filtered
    /// </summary>
    public string Value { get; set; }  = default!;
}