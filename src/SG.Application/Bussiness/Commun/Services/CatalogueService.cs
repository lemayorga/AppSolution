using AutoMapper;
using SG.Domain.Commun.Entities;
using SG.Domain;
using SG.Application.Bussiness.Commun.Dtos;
using SG.Application.Bussiness.Commun.Intefaces;
using Microsoft.Extensions.Logging;

namespace SG.Application.Bussiness.Commun.Services;

public class CatalogueService : BaseGenericService<Catalogue, CatalogueDto>,  ICatalogueService
{
    public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CatalogueService> logger) : base(unitOfWork, mapper, logger)
    {
    }
}
