using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Data.Extensions;

namespace SG.Application.Bussiness.Security.Services;

public class RoleService : BaseGenericService<Role, RoleDto, RoleCreateDto, RoleUpdateDto>, IRoleService
{
    public RoleService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<RoleService> logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<Result<bool>> AddUsersToRole(int idRole, List<int> listIdUsers)
    {
        var result = await _unitOfWork.RoleRepository.AddUsersToRole(idRole, listIdUsers);
        return Result.Ok(result);
    }

    public async Task<Result<bool>> AddUsersToRole(string codeRole, List<int> listIdUsers)
    {
        var result = await _unitOfWork.RoleRepository.AddUsersToRole(codeRole, listIdUsers);
        return Result.Ok(result);
    }

    public async Task<Result<IEnumerable<UserRolesDto>>> GetFilterUsersAndRoles(FilterUsersRoles filters)
    {
        Expression<Func<Role, bool>> _whereRoles = f => true;
        Expression<Func<User, bool>> _whereUsers = f => true;

        if (filters.IdRol.HasValue)
            _whereRoles = _whereRoles.And(x => x.Id == filters.IdRol.Value);

        if (!string.IsNullOrWhiteSpace(filters.CodeRol))
            _whereRoles = _whereRoles.And(x => x.CodeRol == filters.CodeRol);

        if (filters.IdUser.HasValue)
            _whereUsers = _whereUsers.And(x => x.Id == filters.IdUser.Value);

        var listUserRoles = (from ur in _unitOfWork.UsersRolesRepository.GetAll(f => f.State)
                             join role in _unitOfWork.RoleRepository.GetAll().Where(_whereRoles).AsSplitQuery() on ur.IdRol equals role.Id
                             join user in _unitOfWork.UserRepository.GetAll().Where(_whereUsers).AsSingleQuery() on ur.IdUser equals user.Id
                             select new UserRolesDto
                             {
                                 IdUser = user.Id,
                                 UserName = user.Username,
                                 UserEmail = user.Email!,
                                 IdRol = role.Id,
                                 RolName = role.Name!
                             }).Distinct().AsEnumerable();

        var result = await Task.FromResult(listUserRoles);
        return Result.Ok(result);
    }
}