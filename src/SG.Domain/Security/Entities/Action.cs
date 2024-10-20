using System;

namespace SG.Domain.Security.Entities;

public class Action : Entity
{
    public string Name { get; set; } = string.Empty;

    public bool State { get; set; }

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
