using FluentResults;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Roles.Requests;
using SG.Application.Bussiness.Security.Roles.Responses;

namespace SG.Application.Bussiness.Security.Roles.Interfaces;

public interface IRoleService : IServices, IBaseGenericService<Domain.Security.Entities.Role, RoleResponse, RoleCreateRequest, RoleUpdateRequest>
{
    Task<Result<bool>> AddUsersToRole(int idRole, List<int> listIdUsers);
    Task<Result<bool>> AddUsersToRole(string codeRole, List<int> listIdUsers);
    Task<Result<IEnumerable<UserRolesResponse>>> GetFilterUsersAndRoles(FilterUsersRolesRequest filters);
}