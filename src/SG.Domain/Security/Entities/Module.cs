using System;

namespace SG.Domain.Security.Entities;

public class Module  : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool State { get; set; }
    public int? IdParentModule { get; set; }
    public Module? ParentModule { get; set; } = null;
    public ICollection<Module> ChildrenModules { get; set; } = new List<Module>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
