namespace SG.Application.Bussiness.Security.Roles.Requests;

public class RoleUpdateRequest : RoleBaseRequest
{
    public bool IsActive { get; set; }
}
