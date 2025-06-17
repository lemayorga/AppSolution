using System;using FluentValidation;
using SG.Shared.Constants;

namespace SG.Application.Behaviours.Commun.Catalogue;


public class CatalogueCreateValidator : AbstractValidator<CatalogueCreateCommand>
{
    public CatalogueCreateValidator()
    {
        RuleFor(v => v.Group).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).NotEmpty();
        RuleFor(v => v.Value).MaximumLength(200).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).NotEmpty();
    }
}

public class CatalogueUpdateValidator  : AbstractValidator<CatalogueUpdateCommand>
{
    public CatalogueUpdateValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
        RuleFor(v => v.Group).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).NotEmpty();
        RuleFor(v => v.Value).MaximumLength(200).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).NotEmpty();
    }
}

public class CatalogueRemoveByIdValidator  : AbstractValidator<CatalogueRemoveByIdCommand>
{
    public CatalogueRemoveByIdValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
    }
}

