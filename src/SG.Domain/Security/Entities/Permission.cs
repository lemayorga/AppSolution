using System;

namespace SG.Domain.Security.Entities;

public class Permission : Entity
{
    public int IdRol { get; set; }
    public int IdModule { get; set; }
    public int IdAction { get; set; }
    public bool State { get; set; }

    public Module? Module { get; set; }
    public Action? Action { get; set; }
}
