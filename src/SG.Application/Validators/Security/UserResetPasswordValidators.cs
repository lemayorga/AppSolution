

using FluentValidation;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Shared.Constants;

namespace SG.Application.Validators.Security;

public class UserResetPasswordValidators : AbstractValidator<UserResetPasswordRequest>
{
    public UserResetPasswordValidators()
    {
        RuleFor(v => v.UserName)
        .MaximumLength(DataSchemaConstants.VALUE_USERNAME_MAX_LENGTH)
        .MinimumLength(DataSchemaConstants.VALUE_USERNAME_MIN_LENGTH).NotEmpty().NotNull();
    }
}

public class UserResetPasswordByIdValidators : AbstractValidator<UserResetPasswordByIdRequest>
{
    public UserResetPasswordByIdValidators()
    {
        RuleFor(v => v.UserId)
         .GreaterThan(0)
         .NotNull();
    }
}
