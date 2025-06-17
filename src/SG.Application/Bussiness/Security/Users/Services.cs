using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Logging;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Users.Interfaces;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Application.Bussiness.Security.Users.Responses;
using SG.Domain.Base;
using SG.Domain.Entities.Security;
using SG.Shared.Constants;
using SG.Shared.Extensions;
using SG.Shared.Helpers;
using SG.Shared.Responses;

namespace SG.Application.Bussiness.Security.Users.Services;

public class UserService : BaseGenericService<User, UserResponse, UserCreateRequest, UserUpdateRequest>, IUserService
{

    private const int PASSWORD_RANDOM_MIN_LENGH = 8;
    private const int PASSWORD_RANDOM_MAX_LENGH = 10;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<UserService> logger) 
    : base(unitOfWork, mapper, logger) {  }

    public override async Task<Result<SuccessWithIdResponse>> AddSave(UserCreateRequest modelDto)
    {
       var errorMessages =  await ValidationCreateUser(modelDto);
        if(errorMessages.Any())
        {
            return Result.Fail<SuccessWithIdResponse>(errorMessages);
        }

        modelDto.Username = modelDto.Username.Trim();
        modelDto.Firstname = modelDto.Firstname.Trim();
        modelDto.Lastname = modelDto.Lastname.Trim();
        modelDto.Password = modelDto.Password.Trim();
        modelDto.Password = EncryptionUtils.HashText(modelDto.Password);       
        
        return await base.AddSave(modelDto);
    }
    

    public override async Task<Result<List<SuccessWithIdResponse>>> AddManySave(List<UserCreateRequest> modelDto)
    {
        var errorMessages = new List<string>();
        for(var i = 0; i  < modelDto.Count() ; i ++)
        {
            var _errorMessages = await ValidationCreateUser(modelDto[i]);
            if(!_errorMessages.Any()) 
            {
                modelDto[i].Password = EncryptionUtils.HashText(modelDto[i].Password);   
            }
            else 
            {
                errorMessages.AddRange(_errorMessages);
                modelDto.RemoveAt(i);
            }
        }

        if(errorMessages.Any())
        {
            return Result.Fail<List<SuccessWithIdResponse>>(errorMessages);
        }

        return await base.AddManySave(modelDto);
    }

    private  async Task<HashSet<string>> ValidationCreateUser(UserCreateRequest userToCreate) 
    {
        var resultFilterEmail = await _unitOfWork.UserRepository.FilterByOrEmail(userToCreate.Email);
        var resultFilterUsername = await _unitOfWork.UserRepository.FilterByUserName(userToCreate.Username);

        var errorMessages = new HashSet<string>();
        if(resultFilterUsername.Any())
        {
            errorMessages.Add($"{MESSAGE_CONSTANTES.VALIDATION_USER_NAME_REGISTERED.RemoveLastChar('.')}: {userToCreate.Username} .");
        }

        if(resultFilterEmail.Any())
        {
            errorMessages.Add($"{MESSAGE_CONSTANTES.VALIDATION_USER_EMAIL_REGISTERED.RemoveLastChar('.')}: {userToCreate.Email} .");
        }

        return errorMessages;
    }

    public override async Task<Result<SuccessWithIdResponse>> UpdateById(int id, UserUpdateRequest modelDto)
    {
        var user = await _unitOfWork.UserRepository.GetById(id);
        if(user is null)
        {
            return Result.Fail(MESSAGE_CONSTANTES.NOT_ITEM_FOUND_DATABASE);
        }  

        user.Username = modelDto.Username.Trim();
        user.Email = modelDto.Email.Trim();
        user.Firstname = modelDto.Firstname.Trim();
        user.Lastname = modelDto.Lastname.Trim();
        user.IsLocked = modelDto.IsLocked;
        user.IsActive = modelDto.IsActive;
        var result = await _unitOfWork.UserRepository.UpdateAndSave(user);

        return Result.Ok(new SuccessWithIdResponse(id));   
    }

    public async Task<Result<bool>> ChangePassword(UserChangePasswordRequest  model)
    {
        var (userName, currentPassword, newPassword) = model;
        var userResult = (model.EvaluateEmail.HasValue && model?.EvaluateEmail == true)
                            ? (await _unitOfWork.UserRepository.FilterByUserNameOrEmail(userName))
                            : (await _unitOfWork.UserRepository.FilterByUserName(userName));

        if (!userResult.Any())
        {
            return Result.Fail<bool>(MESSAGE_CONSTANTES.VALIDATION_USER_DOESNT_EXIST);
        }

        var user = userResult.ElementAt(0);
        bool resultComparePassword = EncryptionUtils.Verify(user.Password, currentPassword);
        if (!resultComparePassword)
        {
            return Result.Fail<bool>(MESSAGE_CONSTANTES.VALIDATION_CURRENT_PASSWORD_NOT_MATCH);
        }

        string newPasswordHash = EncryptionUtils.HashText(newPassword);
        var result = await _unitOfWork.UserRepository.UpdatePasswordHash(user.Id, newPasswordHash);
        return Result.Ok(result);
    }

    public async Task<Result<bool>> UpdateStatusIsLocked(int idUser, bool newStatusLocked)
    {
        var result = await _unitOfWork.UserRepository.UpdateStatusIsLocked(idUser, newStatusLocked);
        return Result.Ok(result);  
    }

    public async Task<Result<bool>> UpdateStatusIsActived(int idUser, bool newStatusActive)
    {
        var result = await _unitOfWork.UserRepository.UpdateStatusIsActived(idUser, newStatusActive);
        return Result.Ok(result);    
    }   

    public async Task<Result<bool>> ResetPassword(UserResetPasswordRequest model)
    {
        try
        {   
            var userName  = model.UserName.Trim();
            var userResult =  (model.EvaluateEmail.HasValue && model?.EvaluateEmail == true)
                            ?  (await _unitOfWork.UserRepository.FilterByUserNameOrEmail(userName))
                            :  (await _unitOfWork.UserRepository.FilterByUserName(userName));

            string messageError = !userResult.Any() ? MESSAGE_CONSTANTES.VALIDATION_USER_DOESNT_EXIST
                                : !userResult.ElementAt(0).IsActive ? MESSAGE_CONSTANTES.VALIDATION_USER_IS_BLOCKED
                                : userResult.ElementAt(0).IsLocked ? MESSAGE_CONSTANTES.VALIDATION_USER_IS_BLOCKED
                                : string.Empty;   

            if (!string.IsNullOrWhiteSpace(messageError))
            {
                return Result.Fail<bool>(messageError);
            }

            var user = userResult.ElementAt(0);
            string newPassword = RandomPassword.Generate(PASSWORD_RANDOM_MIN_LENGH, PASSWORD_RANDOM_MAX_LENGH);
            string newPasswordHash = EncryptionUtils.HashText(newPassword);      
            var result = await _unitOfWork.UserRepository.UpdatePasswordHash(user.Id, newPasswordHash);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Fail(ex.Message);
        }    
    }    
    public async Task<Result<bool>> ResetPasswordBydIdUser(int idUser)
    {
 
        string newPassword = RandomPassword.Generate(PASSWORD_RANDOM_MIN_LENGH, PASSWORD_RANDOM_MAX_LENGH);
        string newPasswordHash = EncryptionUtils.HashText(newPassword);      
        var result = await _unitOfWork.UserRepository.UpdatePasswordHash(idUser, newPasswordHash);
        return Result.Ok(result);   
    }    
}
