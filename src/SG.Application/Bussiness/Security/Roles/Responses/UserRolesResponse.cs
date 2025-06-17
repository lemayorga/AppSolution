using System;

namespace SG.Application.Bussiness.Security.Roles.Responses;

public class UserRolesResponse
{
    public int IdUser { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public string RolName { get; set; } = string.Empty;
}
