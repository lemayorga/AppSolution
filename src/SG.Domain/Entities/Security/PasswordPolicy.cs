using SG.Domain.Base;

namespace SG.Domain.Entities.Security;

public class PasswordPolicy  : BaseEntity<int>
{ 
    public  string Code { get; set; } = default!;
    public  string Value { get; set; } = default!;
    public string? Description { get; set; } = default!;
}
