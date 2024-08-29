using AutoMapper;
using SG.Application.Bussiness.Commun.Dtos;
using SG.Domain.Commun.Entities;

namespace SG.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogueDto, Catalogue>().ReverseMap();
    }
}