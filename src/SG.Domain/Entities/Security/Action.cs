using SG.Domain.Base;

namespace SG.Domain.Entities.Security;

public class Action : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public ICollection<Permission> Permissions { get; set; } = [];

    public Action() { }

    public Action(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }

    public Action(string name, bool isActive, IList<Permission> permissions)
    {
        Name = name;
        IsActive = isActive;
        Permissions = permissions;
    }
}
