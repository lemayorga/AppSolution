using SG.Domain;

namespace SG.Domain.Commun.Entities;

public class Company : Entity
{
    public required string Name { get; set; }
    public required bool IsActive { get; set; }
    public int? IdMayorCompany { get; set; }   
    public Company? MayorCompany { get; set; }   
}
