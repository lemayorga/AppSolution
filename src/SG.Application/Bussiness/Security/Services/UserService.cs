using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Domain.Security.Entities;
using SG.Shared.Helpers;

namespace SG.Application.Bussiness.Security.Services;

public class UserService : BaseGenericService<User, UserDto, UserCreateDto, UserUpdateDto>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<UserService> logger) : base(unitOfWork, mapper, logger)
    {        
    }

    public override async Task<Result<UserDto>> AddSave(UserCreateDto modelDto)
    {
        var resultFilterEmail = await _unitOfWork.UserRepository.FilterByOrEmail(modelDto.Email);
        var resultFilterUsername = await _unitOfWork.UserRepository.FilterByUserName(modelDto.Username);

        var errorMessages = Enumerable.Empty<string>();

        if(resultFilterUsername.Any())
        {
            errorMessages.Append("Username is already registered");
        }

        if(resultFilterEmail.Any())
        {
           errorMessages.Append("Email is already registered");
        }

        if(errorMessages.Any())
        {
            return Result.Fail<UserDto>(errorMessages);
        }

        modelDto.Username = modelDto.Username.Trim();
        modelDto.Firstname = modelDto.Firstname.Trim();
        modelDto.Lastname = modelDto.Lastname.Trim();
        modelDto.Password = modelDto.Password.Trim();
        modelDto.Password = EncryptionUtils.HashText(modelDto.Password);       
        
        return await base.AddSave(modelDto);
    }

}

