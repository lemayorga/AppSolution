using FluentValidation;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Shared.Constants;

namespace SG.Application.Validators.Security;


public class UserChangePasswordValidator : AbstractValidator<UserChangePasswordRequest>
{
    public UserChangePasswordValidator()
    {
        RuleFor(v => v.UserName)
        .MaximumLength(DataSchemaConstants.VALUE_USERNAME_MAX_LENGTH)
        .MinimumLength(DataSchemaConstants.VALUE_USERNAME_MIN_LENGTH).NotEmpty().NotNull();

        RuleFor(v => v.CurrentPassword)
            .NotEmpty().WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_IS_REQUIRED)
            .MinimumLength(DataSchemaConstants.VALUE_PASWORD_MIN_LENGTH).WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MIN_LENGTH)
            .Matches("[A-Z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_UPPERCASE)
            .Matches("[a-z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_LOWERCASE)
            .Matches("[0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_DIGIT)
            .Matches("[^a-zA-Z0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CHARACTER);
       
        RuleFor(v => v.CurrentPassword)
            .NotEmpty().WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_IS_REQUIRED)
            .MinimumLength(DataSchemaConstants.VALUE_PASWORD_MIN_LENGTH).WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MIN_LENGTH)
            .Matches("[A-Z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_UPPERCASE)
            .Matches("[a-z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_LOWERCASE)
            .Matches("[0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_DIGIT)
            .Matches("[^a-zA-Z0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CHARACTER);

        RuleFor(v => v.NewPassword)           
            .NotEmpty().WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_IS_REQUIRED)
            .MinimumLength(DataSchemaConstants.VALUE_PASWORD_MIN_LENGTH).WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MIN_LENGTH)
            .Matches("[A-Z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_UPPERCASE)
            .Matches("[a-z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_LOWERCASE)
            .Matches("[0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_DIGIT)
            .Matches("[^a-zA-Z0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CHARACTER);
    }
}