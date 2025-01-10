using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SG.Domain;
using SG.Domain.Commun.Repositories;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data;
using SG.Infrastructure.Data.Config;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Repositories.Commun;
using SG.Infrastructure.Data.Repositories.Security;
using SG.Infrastructure.DatabaseFlavor;
using SG.Shared.Enumerators;

namespace SG.Infrastructure.Services;

public static class DatabaseDependencyInjection
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
         // Obtener el proveedor de base de datos desde la configuración
        var databaseProvider = configuration.GetValue<string>("DatabaseProvider");         
        ArgumentNullException.ThrowIfNull(databaseProvider);

        var connectionStrings =  configuration.GetValue<string>($"ConnectionStrings:{databaseProvider}"); 
        ArgumentNullException.ThrowIfNull(connectionStrings);

        var optionsAction = databaseProvider switch
        {
            nameof(EnumGestoresBD.SqlServer) => ProviderSelector.WithProviderAutoSelection(EnumGestoresBD.SqlServer, connectionStrings),
            nameof(EnumGestoresBD.PostgreSql) => ProviderSelector.WithProviderAutoSelection(EnumGestoresBD.PostgreSql, connectionStrings),
            _ => throw new NotImplementedException(),
        };
             
        services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>(sp => new DatabaseConfiguration(databaseProvider, connectionStrings));
        services.AddDbContext<ApplicationDbContext>(optionsAction);

        return services;
    }

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
                .AddScoped<IPermissionRepository, PermissionRepository>()
                .AddScoped<IUsersRolesRepository, UsersRolesRepository>();     
        #endregion
 
        
        return services;
    }
}
