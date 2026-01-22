using System;
using FluentValidation;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Shared.Constants;

namespace SG.Application.Validators.Commun;

public class CatalogueCreateValidator : AbstractValidator<CatalogueCreateRequest>
{
    public CatalogueCreateValidator()
    {
        RuleFor(v => v.Value).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_MEDIUM_TEXT).NotEmpty();
        RuleFor(v => v.Code).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT);
    }
}

public class CatalogueCreateEnumerableValidator : AbstractValidator<List<CatalogueCreateRequest>>
{
    public CatalogueCreateEnumerableValidator()
    {
        RuleForEach(model => model).SetValidator(new CatalogueCreateValidator());
    }
}

public class CatalogueUpdateValidator : AbstractValidator<CatalogueUpdateRequest>
{
    public CatalogueUpdateValidator()
    {
        RuleFor(v => v.Value).MaximumLength(DataSchemaConstants.DEFAULT_LENGTH_MEDIUM_TEXT).NotEmpty();
        RuleFor(v => v.Code).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT);
    }
}

public class CatalogueUpdateEnumerableValidator : AbstractValidator<List<CatalogueUpdateRequest>>
{
    public CatalogueUpdateEnumerableValidator()
    {
        RuleForEach(model => model).SetValidator(new CatalogueUpdateValidator());
    }
}