using SG.Domain.Base;

namespace SG.Domain.Entities.Commun;

public class Company : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public int? IdMayorCompany { get; set; }
    public Company? MayorCompany { get; set; }

    public Company() { }

    public Company(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }

    public Company(string name, bool isActive, int idMayorCompany)
    {
        Name = name;
        IsActive = isActive;
        IdMayorCompany = idMayorCompany;
    }

    public Company(string name, bool isActive, int idMayorCompany, Company mayorCompany)
    {
        Name = name;
        IsActive = isActive;
        IdMayorCompany = idMayorCompany;
        MayorCompany = mayorCompany;
    }
}
