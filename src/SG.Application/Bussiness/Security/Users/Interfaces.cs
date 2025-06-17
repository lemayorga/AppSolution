using FluentResults;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Application.Bussiness.Security.Users.Responses;

namespace SG.Application.Bussiness.Security.Users.Interfaces;

public interface IUserService : IServices, IBaseGenericService<Domain.Security.Entities.User, UserResponse, UserCreateRequest, UserUpdateRequest>
{
    Task<Result<bool>> ChangePassword(UserChangePasswordRequest model);
    Task<Result<bool>> ResetPassword(UserResetPasswordRequest model);
    Task<Result<bool>> ResetPasswordBydIdUser(int idUser);
    Task<Result<bool>> UpdateStatusIsLocked(int idUser, bool newStatusLocked);
    Task<Result<bool>> UpdateStatusIsActived(int idUser, bool newStatusActive);
}