using SG.Domain.Base;

namespace SG.Domain.Entities.Security;


public class Role : BaseEntity<int>
{
    public  string CodeRol { get; set; }  = default!;
    public  string Name { get; set; } = default!;
    
    public  bool IsActive { get; set; }
    public ICollection<UsersRoles>? UsersRoles { get; set; }
}
