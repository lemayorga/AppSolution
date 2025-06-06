using SG.Domain.Base;

namespace SG.Domain.Security.Entities;

public class UsersRoles : BaseEntity<int>
{
    public int IdUser { get; set; }    
    public int IdRol { get; set; }   
    public bool State { get; set; }
    public User User { get; set; }   = default!;
    public Role Role { get; set; }  = default!;
}
