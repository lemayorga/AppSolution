using SG.Domain.Entities.Security;
using SG.Shared.Responses;

namespace SG.Application.Bussiness.Security.Roles.Responses;

public class RoleResponse : BaseWithIdResponse
{
    public string CodeRol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }


    public RoleResponse(){ }
    public RoleResponse(int id, string codeRol, string name, bool isActive)
    {
        Id = id;
        CodeRol = codeRol;
        Name = name;
        IsActive = isActive;
    }

    public RoleResponse(Role model) : this
    (
        id: model.Id,
        codeRol: model.CodeRol,
        name: model.Name,
        isActive: model.IsActive
    ){ }
}
