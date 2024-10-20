using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Domain.Security.Entities;

namespace SG.Application.Bussiness.Security.Services;

public class RoleService : BaseGenericService<Role, RoleDto, RoleCreateDto, RoleUpdateDto>, IRoleService
{
    public RoleService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<RoleService> logger) : base(unitOfWork, mapper, logger)
    {
    }
}
