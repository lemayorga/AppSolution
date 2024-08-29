using SG.Domain;

namespace SG.Domain.Security.Entities;

public class UsersRoles : Entity
{
    public required int IdUser { get; set; }    
    public required int IdRol { get; set; }   
}
