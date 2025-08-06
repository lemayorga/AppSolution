
using SG.Shared.Enumerators;

namespace SG.Infrastructure.Base.Pagination;

public class FilterParam
{
    public string Field { get; set; } = default!;
    public EnumOperatorFilter Operator { get; set; }
    public string Value { get; set; } = default!;
}
