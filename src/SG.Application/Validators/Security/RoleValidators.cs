using FluentValidation;
using SG.Application.Bussiness.Security.Roles.Requests;
using SG.Shared.Constants;

namespace SG.Application.Validators.Security;


public class RoleCreateValidator : AbstractValidator<RoleCreateRequest>
{
    public RoleCreateValidator()
    {
        RuleFor(v => v.CodeRol).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SMALL_TEXT).NotEmpty().NotNull();
        RuleFor(v => v.Name).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty().NotNull();
    }
}

public class RoleCreateEnumerableValidator : AbstractValidator<List<RoleCreateRequest>>
{
    public RoleCreateEnumerableValidator()
     => RuleForEach(model => model).SetValidator(new RoleCreateValidator());
}

public class RoleUpdateValidator : AbstractValidator<RoleUpdateRequest>
{
    public RoleUpdateValidator()
    {
        RuleFor(v => v.CodeRol).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SMALL_TEXT).NotEmpty().NotNull();
        RuleFor(v => v.Name).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).NotEmpty().NotNull();
        RuleFor(v => v.IsActive).NotNull();
    }
}

public class RoleUpdateEnumerableValidator : AbstractValidator<List<RoleUpdateRequest>>
{
    public RoleUpdateEnumerableValidator()
     => RuleForEach(model => model).SetValidator(new RoleUpdateValidator());
}