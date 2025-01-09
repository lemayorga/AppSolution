using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Domain.Security.Entities;
using SG.Shared.Constants;
using SG.Shared.Helpers;

namespace SG.Application.Bussiness.Security.Services;

public class UserService : BaseGenericService<User, UserDto, UserCreateDto, UserUpdateDto>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<UserService> logger) : base(unitOfWork, mapper, logger) {  }

    public override async Task<Result<UserDto>> AddSave(UserCreateDto modelDto)
    {
        var resultFilterEmail = await _unitOfWork.UserRepository.FilterByOrEmail(modelDto.Email);
        var resultFilterUsername = await _unitOfWork.UserRepository.FilterByUserName(modelDto.Username);

        var errorMessages = Enumerable.Empty<string>();

        if(resultFilterUsername.Any())
        {
            errorMessages.Append(MESSAGE_CONSTANTES.USER_NAME_REGISTERED);
        }

        if(resultFilterEmail.Any())
        {
           errorMessages.Append(MESSAGE_CONSTANTES.USER_EMAIL_REGISTERED);
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

