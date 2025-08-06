using FluentResults;
using FluentValidation;
using SG.Application.Base.CQRS;
using SG.Application.Extensions;
using SG.Domain.Repositories.Commun;
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

        var model = new Domain.Entities.Commun.Catalogue
        (
           group: command.Group,
           value: command.Value,
           isActive: command.IsActive,
           description: command.Description
        );

        await repository.Add(model);
        await repository.SaveChangesAsync();

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

        var model = new Domain.Entities.Commun.Catalogue
        (
           id: command.Id,
           group: command.Group,
           value: command.Value,
           isActive: command.IsActive,
           description: command.Description
        );

        var result = await repository.UpdateById(command.Id, model);
        if (!result)
        {
            return Result.Fail(MESSAGE_CONSTANTS.NOT_ITEM_FOUND_DATABASE);
        }

        await repository.SaveChangesAsync();
        return Result.Ok(new SuccessWithIdResponse(model.Id));
    }
}



public sealed class CatalogueRemoveByIdCommandHandler(ICatalogueRepository repository) : IRequestHandler<CatalogueRemoveByIdCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CatalogueRemoveByIdCommand command)
    {
        var result = await repository.DeleteById(command.Id);
        if (!result)
        {
            return Result.Fail(MESSAGE_CONSTANTS.NOT_ITEM_FOUND_DATABASE);
        }
        
        await repository.SaveChangesAsync();
        return Result.Ok(true);
    }
}


