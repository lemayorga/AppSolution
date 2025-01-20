using FluentResults;
using SG.Application.Bussiness.Security.Dtos;
using SG.Domain.Security.Entities;

namespace SG.Application.Bussiness.Security.Interfaces;

public interface IUserService : IBaseGenericService<User, UserDto, UserCreateDto, UserUpdateDto>
{
    Task<Result<bool>> ChangePassword(UserChangePassword model);
    Task<Result<bool>> ResetPassword(UserResetPassword model);
    Task<Result<bool>> ResetPasswordBydIdUser(int idUser);
    Task<Result<bool>> UpdateStatusIsLocked(int idUser, bool newStatusLocked);
    Task<Result<bool>> UpdateStatusIsActived(int idUser, bool newStatusActive);
}