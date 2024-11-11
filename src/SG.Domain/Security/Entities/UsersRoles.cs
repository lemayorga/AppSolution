using SG.Domain;

namespace SG.Domain.Security.Entities;

public class UsersRoles : Entity
{
    public int IdUser { get; set; }    
    public int IdRol { get; set; }   
    public bool State { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }
}
