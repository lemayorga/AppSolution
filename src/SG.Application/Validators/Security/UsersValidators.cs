using FluentValidation;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Shared.Constants;

namespace SG.Application.Validators.Security;


public class UserCreateValidator : AbstractValidator<UserCreateRequest>
{
    public UserCreateValidator()
    {
        RuleFor(v => v.Username)
        .MaximumLength(DataSchemaConstants.VALUE_USERNAME_MAX_LENGTH)
        .MinimumLength(DataSchemaConstants.VALUE_USERNAME_MIN_LENGTH).NotEmpty().NotNull();

        RuleFor(v => v.Email).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_EMAILS).NotEmpty();
        RuleFor(v => v.Firstname).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty();
        RuleFor(v => v.Lastname).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_IS_REQUIRED)
            .MinimumLength(DataSchemaConstants.VALUE_PASWORD_MIN_LENGTH).WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MIN_LENGTH)
            .Matches("[A-Z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_UPPERCASE)
            .Matches("[a-z]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_LOWERCASE)
            .Matches("[0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CONTAIN_DIGIT)
            .Matches("[^a-zA-Z0-9]").WithMessage(MESSAGE_CONSTANTS.VALIDATION_PASWORD_MUST_CHARACTER);
    }
}

public class UserCreateEnumerableValidator : AbstractValidator<List<UserCreateRequest>>
{
    public UserCreateEnumerableValidator()
        => RuleForEach(model => model).SetValidator(new UserCreateValidator());
}

public class UserUpdateValidator : AbstractValidator<UserUpdateRequest>
{
    public UserUpdateValidator()
    {
        RuleFor(v => v.Username).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty().NotNull();
        RuleFor(v => v.Email).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_EMAILS).NotEmpty().NotNull();
        RuleFor(v => v.Firstname).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty().NotNull();
        RuleFor(v => v.Lastname).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty().NotNull();
    }
}

public class UserUpdateEnumerableValidator : AbstractValidator<List<UserUpdateRequest>>
{
    public UserUpdateEnumerableValidator()
        =>  RuleForEach(model => model).SetValidator(new UserUpdateValidator());
}