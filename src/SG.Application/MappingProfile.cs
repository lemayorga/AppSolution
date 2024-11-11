using AutoMapper;
using SG.Application.Bussiness.Commun.Dtos;
using SG.Application.Bussiness.Security.Dtos;
using SG.Domain.Commun.Entities;
using SG.Domain.Security.Entities;

namespace SG.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Commun
        
        CreateMap<CatalogueDto, Catalogue>().ReverseMap();
        CreateMap<CatalogueCreateDto, Catalogue>().ReverseMap();
        CreateMap<CatalogueUpdateDto, Catalogue>().ReverseMap();  

        #endregion

        #region Security

        CreateMap<RoleDto, Role>().ReverseMap();
        CreateMap<RoleCreateDto, Role>().ReverseMap();
        CreateMap<RoleUpdateDto, Role>().ReverseMap(); 


        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UserCreateDto, User>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap(); 

        #endregion

    }
}
