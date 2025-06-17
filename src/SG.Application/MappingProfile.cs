using AutoMapper;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Application.Bussiness.Security.Roles.Requests;
using SG.Application.Bussiness.Security.Roles.Responses;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Application.Bussiness.Security.Users.Responses;
using SG.Domain.Entities.Commun;
using SG.Domain.Entities.Security;
namespace SG.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Commun
        
        CreateMap<CatalogueResponse, Catalogue>().ReverseMap();
        CreateMap<CatalogueCreateRequest, Catalogue>().ReverseMap();
        CreateMap<CatalogueUpdateRequest, Catalogue>().ReverseMap();  

        #endregion

        #region Security

        CreateMap<RoleResponse, Role>().ReverseMap();
        CreateMap<RoleCreateRequest, Role>().ReverseMap();
        CreateMap<RoleUpdateRequest, Role>().ReverseMap(); 


        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<UserCreateRequest, User>().ReverseMap();
        CreateMap<UserUpdateRequest, User>().ReverseMap(); 

        #endregion

    }
}
