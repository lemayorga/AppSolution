using Microsoft.Extensions.DependencyInjection;
using SG.Application.Bussiness;
using SG.Application.Bussiness.Commun.Intefaces;
using SG.Application.Bussiness.Commun.Services;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Application.Bussiness.Security.Services;

namespace SG.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesApplication(this IServiceCollection services) 
    {
        services.AddScoped(typeof(IBaseGenericService<,,,>), typeof(BaseGenericService<,,,>));

        #region Commun
        
        services.AddScoped<ICatalogueService, CatalogueService>();       

        #endregion

        #region Security
        
        services.AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>();     
        #endregion
 
        
        return services;
    }
               
}
