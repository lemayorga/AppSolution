using Microsoft.Extensions.DependencyInjection;
using SG.Domain;
using SG.Domain.Commun.Repositories;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Repositories.Commun;
using SG.Infrastructure.Data.Repositories.Security;

namespace SG.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
    {
        services.AddScoped<ApplicationDbContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>();      
        
        #region Commun
        
        services.AddScoped<ICatalogueRepository, CatalogueRepository>();       

        #endregion

        #region Security
        services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>()
                .AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IActionRepository, ActionRepository>()
                .AddScoped<IModuleRepository, ModuleRepository>()
                .AddScoped<IPermissionRepository, PermissionRepository>();     
        #endregion
 
        
        return services;
    }
}
