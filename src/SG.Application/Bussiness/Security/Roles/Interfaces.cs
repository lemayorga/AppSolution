using FluentResults;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Roles.Requests;
using SG.Application.Bussiness.Security.Roles.Responses;
using SG.Domain.Entities.Security;

namespace SG.Application.Bussiness.Security.Roles.Interfaces;

public interface IRoleService : IServices, IBaseGenericService<Role, RoleResponse, RoleCreateRequest, RoleUpdateRequest>
{
    Task<Result<bool>> AddUsersToRole(int idRole, List<int> listIdUsers);
    Task<Result<bool>> AddUsersToRole(string codeRole, List<int> listIdUsers);
    Task<Result<IEnumerable<UserRolesResponse>>> GetFilterUsersAndRoles(FilterUsersRolesRequest filters);
}