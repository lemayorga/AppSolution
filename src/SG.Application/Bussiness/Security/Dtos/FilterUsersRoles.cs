using System;

namespace SG.Application.Bussiness.Security.Dtos;

public class FilterUsersRoles
{
    public int? IdRol { get; set; }
    public string? CodeRol { get; set; }
    public int? IdUser { get; set; }
}