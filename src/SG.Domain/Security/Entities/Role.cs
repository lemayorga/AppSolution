using SG.Domain;

namespace SG.Domain.Security.Entities;


public class Role  : Entity
{
    public required string CodeRol { get; set; }
    public required string Name { get; set; }
    public required bool IsActive { get; set; }
}
