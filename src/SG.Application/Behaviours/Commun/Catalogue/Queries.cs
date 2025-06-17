using System;
using FluentResults;
using SG.Application.Base.CQRS;
using SG.Shared.Request;

namespace SG.Application.Behaviours.Commun.Catalogue;

public class GetCatalogueAllQuery : IRequest<Result<List<CatalogueResponse>>> { }

public class GetCatalogueByIdQuery(int id) : BaseWithIdRequest(id), IRequest<Result<CatalogueResponse>>{ }