using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Domain.Security.Entities;

namespace SG.Application.Bussiness.Security.Services;

public class UserService : BaseGenericService<User, UserDto, UserCreateDto, UserUpdateDto>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<UserService> logger) : base(unitOfWork, mapper, logger)
    {
    }
}
