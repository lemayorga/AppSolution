using System;

namespace SG.Infrastructure.Auth.JwtAuthentication.Models;

public sealed class JwtRolData
{
    public int IdRol { get; set; }
    public string Name { get; set; } = default!;

    public JwtRolData(int idRol, string name)
    {
        this.IdRol = idRol;
        this.Name = name;
    }
}
