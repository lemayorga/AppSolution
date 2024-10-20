using System;
using SG.Application.Bussiness.Security.Dtos;
using SG.Domain.Security.Entities;

namespace SG.Application.Bussiness.Security.Interfaces;

public interface IRoleService : IBaseGenericService<Role, RoleDto, RoleCreateDto, RoleUpdateDto>
{
 
}