using System;
using SG.Domain.Base;

namespace SG.Domain.Security.Entities;

public class Permission : BaseEntity<int>
{
    public int IdRol { get; set; }
    public int IdModule { get; set; }
    public int IdAction { get; set; }
    public bool IsActive { get; set; }

    public Module? Module { get; set; }
    public Action? Action { get; set; }
}
