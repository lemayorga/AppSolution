using FluentResults;
using SG.Application.Base.CQRS;
using SG.Shared.Request;
using SG.Shared.Responses;

namespace SG.Application.Behaviours.Commun.Catalogue;

public abstract class CatalogueBaseCommand 
{
    public string Group { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
}


public class CatalogueCreateCommand : CatalogueBaseCommand, IRequest<Result<SuccessWithIdResponse>>
{
    public bool IsActive { get => true; }
}

public class CatalogueUpdateCommand : CatalogueBaseCommand, IBaseWithIdRequest<int>, IRequest<Result<SuccessWithIdResponse>>
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}


public class CatalogueRemoveByIdCommand(int id) : BaseWithIdRequest(id), IRequest<Result<bool>>
{
}