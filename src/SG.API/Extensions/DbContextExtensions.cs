using SG.Infrastructure.Data.Context;
using SG.Infrastructure.DatabaseFlavor;
using SG.Shared.Enumerators;

namespace SG.API.Extensions;

internal  static  class DbContextExtensions
{
    internal  static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
         // Obtener el proveedor de base de datos desde la configuración
        var databaseProvider = configuration.GetValue<string>("DatabaseProvider"); 
        
        ArgumentNullException.ThrowIfNull(databaseProvider);

        var optionsAction = databaseProvider switch
        {
            nameof(EnumGestoresBD.SqlServer) => ProviderSelector.WithProviderAutoSelection(EnumGestoresBD.SqlServer, databaseProvider),
            nameof(EnumGestoresBD.PostgreSql) => ProviderSelector.WithProviderAutoSelection(EnumGestoresBD.PostgreSql, databaseProvider),
            _ => throw new NotImplementedException(),
        };
             
        services.AddDbContext<ApplicationDbContext>(optionsAction);

        return services;
    }
}