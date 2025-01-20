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

    private const int PASSWORD_RANDOM_MIN_LENGH = 8;
    private const int PASSWORD_RANDOM_MAX_LENGH = 10;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<UserService> logger) 
    : base(unitOfWork, mapper, logger) {  }

    public override async Task<Result<UserDto>> AddSave(UserCreateDto modelDto)
    {
        var resultFilterEmail = await _unitOfWork.UserRepository.FilterByOrEmail(modelDto.Email);
        var resultFilterUsername = await _unitOfWork.UserRepository.FilterByUserName(modelDto.Username);

        var errorMessages = Enumerable.Empty<string>();

        if(resultFilterUsername.Any())
        {
            errorMessages.Append(MESSAGE_CONSTANTES.VALIDATION_USER_NAME_REGISTERED);
        }

        if(resultFilterEmail.Any())
        {
           errorMessages.Append(MESSAGE_CONSTANTES.VALIDATION_USER_EMAIL_REGISTERED);
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

    public async Task<Result<bool>> ChangePassword(UserChangePassword model)
    {
        try
        {
            var (userName, currentPassword, newPassword) = model;
            var userResult =  (model.EvaluateEmail.HasValue && model?.EvaluateEmail == true)
                            ?  (await _unitOfWork.UserRepository.FilterByUserNameOrEmail(userName))
                            :  (await _unitOfWork.UserRepository.FilterByUserName(userName));

            if (!userResult.Any() )
            {
                return Result.Fail<bool>(MESSAGE_CONSTANTES.VALIDATION_USER_DOESNT_EXIST);
            }
            
            var user = userResult.ElementAt(0);
            bool resultComparePassword = EncryptionUtils.Verify(user.Password, currentPassword);
            if(!resultComparePassword)
            {
                return Result.Fail<bool>(MESSAGE_CONSTANTES.VALIDATION_CURRENT_PASSWORD_NOT_MATCH);
            }

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

    public async Task<Result<bool>> UpdateStatusIsLocked(int idUser, bool newStatusLocked)
    {
        try
        {
           var result = await _unitOfWork.UserRepository.UpdateStatusIsLocked(idUser, newStatusLocked);
           return Result.Ok(result);
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, ex.Message);
           return Result.Fail(ex.Message);
        }    
    }

    public async Task<Result<bool>> UpdateStatusIsActived(int idUser, bool newStatusActive)
    {
        try
        {
           var result = await _unitOfWork.UserRepository.UpdateStatusIsActived(idUser, newStatusActive);
           return Result.Ok(result);
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, ex.Message);
           return Result.Fail(ex.Message);
        }    
    }   

    public async Task<Result<bool>> ResetPassword(UserResetPassword model)
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
        try
        {   
            string newPassword = RandomPassword.Generate(PASSWORD_RANDOM_MIN_LENGH, PASSWORD_RANDOM_MAX_LENGH);
            string newPasswordHash = EncryptionUtils.HashText(newPassword);      
            var result = await _unitOfWork.UserRepository.UpdatePasswordHash(idUser, newPasswordHash);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Fail(ex.Message);
        }    
    }    
}
