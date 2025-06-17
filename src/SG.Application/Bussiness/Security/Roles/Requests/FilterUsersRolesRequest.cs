using System;

namespace SG.Application.Bussiness.Security.Roles.Requests;

public class FilterUsersRolesRequest
{
    public int? IdRol { get; set; }
    public string? CodeRol { get; set; }
    public int? IdUser { get; set; }
}
