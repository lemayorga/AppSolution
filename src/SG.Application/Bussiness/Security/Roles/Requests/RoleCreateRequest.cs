using System;

namespace SG.Application.Bussiness.Security.Roles.Requests;

public class RoleCreateRequest : RoleBaseRequest
{
     public bool IsActive { get => true; }
}
