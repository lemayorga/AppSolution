using FluentResults;
using FluentValidation;
using SG.Application.Base.CQRS;
using SG.Application.Base.Validations;
using SG.Application.Extensions;
using SG.Domain.Commun.Repositories;
using SG.Shared.Constants;
using SG.Shared.Responses;

namespace SG.Application.Behaviours.Commun.Catalogue;

public sealed class CreateCatalogueCommandHandler
(
    ICatalogueRepository repository,
    IValidator<CatalogueCreateCommand> validator
): IRequestHandler<CatalogueCreateCommand, Result<SuccessWithIdResponse>>
{
    public async Task<Result<SuccessWithIdResponse>> Handle(CatalogueCreateCommand command)
    {
        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return validationResult.GetResultErrors<SuccessWithIdResponse>();
        }

        var model = new Domain.Commun.Entities.Catalogue
        (
           group: command.Group,
           value: command.Value,
           isActive: command.IsActive,
           description: command.Description
        );

        await repository.AddSave(model);

        return Result.Ok(new SuccessWithIdResponse(model.Id));
    }
}


public sealed class UpdateCatalogueCommandHandler
(
    ICatalogueRepository repository,
    IValidator<CatalogueUpdateCommand> validator
): IRequestHandler<CatalogueUpdateCommand, Result<SuccessWithIdResponse>>
{
    public async Task<Result<SuccessWithIdResponse>> Handle(CatalogueUpdateCommand command)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return validationResult.GetResultErrors<SuccessWithIdResponse>();
        }

        var model = new Domain.Commun.Entities.Catalogue
        (
           id: command.Id,
           group: command.Group,
           value: command.Value,
           isActive: command.IsActive,
           description: command.Description
        );

        var result = await repository.UpdateByIdSave(command.Id, model);
        if (result is null)
        {
            return Result.Fail(MESSAGE_CONSTANTES.NOT_ITEM_FOUND_DATABASE);
        }

        return Result.Ok(new SuccessWithIdResponse(model.Id));
    }
}



public sealed class CatalogueRemoveByIdCommandHandler(ICatalogueRepository repository) : IRequestHandler<CatalogueRemoveByIdCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CatalogueRemoveByIdCommand command)
    {
        var result = await repository.DeleteByIdSave(command.Id);
        if (!result)
        {
            return Result.Fail(MESSAGE_CONSTANTES.NOT_ITEM_FOUND_DATABASE);
        }

        return Result.Ok(true);
    }
}


