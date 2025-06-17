using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Roles.Interfaces;
using SG.Application.Bussiness.Security.Roles.Requests;
using SG.Application.Bussiness.Security.Roles.Responses;
using SG.Domain;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Data.Extensions;

namespace SG.Application.Bussiness.Security.Roles.Service;


public class RoleService : BaseGenericService<Role, RoleResponse, RoleCreateRequest, RoleUpdateRequest>, IRoleService
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

    public async Task<Result<IEnumerable<UserRolesResponse>>> GetFilterUsersAndRoles(FilterUsersRolesRequest filters)
    {
        Expression<Func<Role, bool>> _whereRoles = f => true;
        Expression<Func<Domain.Security.Entities.User, bool>> _whereUsers = f => true;

        if (filters.IdRol.HasValue)
            _whereRoles = _whereRoles.And(x => x.Id == filters.IdRol.Value);

        if (!string.IsNullOrWhiteSpace(filters.CodeRol))
            _whereRoles = _whereRoles.And(x => x.CodeRol == filters.CodeRol);

        if (filters.IdUser.HasValue)
            _whereUsers = _whereUsers.And(x => x.Id == filters.IdUser.Value);

        var listUserRoles = (from ur in _unitOfWork.UsersRolesRepository.GetAll(f => f.State)
                             join role in _unitOfWork.RoleRepository.GetAll().Where(_whereRoles).AsSplitQuery() on ur.IdRol equals role.Id
                             join user in _unitOfWork.UserRepository.GetAll().Where(_whereUsers).AsSingleQuery() on ur.IdUser equals user.Id
                             select new UserRolesResponse
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