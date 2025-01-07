using FluentResults;
using SG.Application.Bussiness.Security.Dtos;
using SG.Domain.Security.Entities;

namespace SG.Application.Bussiness.Security.Interfaces;

public interface IRoleService : IBaseGenericService<Role, RoleDto, RoleCreateDto, RoleUpdateDto>
{
    Task<Result<bool>> AddUsersToRole(int idRole, List<int> listIdUsers);
    Task<Result<bool>> AddUsersToRole(string codeRole, List<int> listIdUsers);
    Task<Result<IEnumerable<UserRolesDto>>> GetFilterUsersAndRoles(FilterUsersRoles filters);
}