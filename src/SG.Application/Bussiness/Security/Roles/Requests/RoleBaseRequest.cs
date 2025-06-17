using System;

namespace SG.Application.Bussiness.Security.Roles.Requests;

public abstract class RoleBaseRequest
{
    public string CodeRol { get; set; } = string.Empty;
    public string Name { get; set; }  = string.Empty;
}
