using FluentResults;
using SG.Application.Base.CQRS;
using SG.Domain.Commun.Repositories;
namespace SG.Application.Behaviours.Commun.Catalogue;

public sealed class GetCatalogueAllQueryHandler(ICatalogueRepository repository) : IRequestHandler<GetCatalogueAllQuery, Result<List<CatalogueResponse>>>
{
    public async Task<Result<List<CatalogueResponse>>> Handle(GetCatalogueAllQuery command)
    {
        var result = repository.GetAllWithSelector(selector: x => new CatalogueResponse(x)).ToList();
        return await Task.FromResult(Result.Ok(result));
    }
}


public sealed class GetCatalogueByIdQueryHandler(ICatalogueRepository repository): IRequestHandler<GetCatalogueByIdQuery, Result<CatalogueResponse>>
{
    public async Task<Result<CatalogueResponse>> Handle(GetCatalogueByIdQuery command)
    {
        var result = await repository.GetOneWithSelector(selector: x => new  CatalogueResponse(x), where: f => f.Id == command.Id);
        return await Task.FromResult(Result.Ok(result!));
    }
}