using Microsoft.EntityFrameworkCore;
using SG.Infrastructure.Data.Context;
using SG.Shared.Enumerators;

namespace SG.API.Extensions;

internal  static  class DbContextExtensions
{
    internal  static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
         // Obtener el proveedor de base de datos desde la configuración
        var databaseProvider = configuration.GetValue<string>("DatabaseProvider");
        
        ArgumentNullException.ThrowIfNull(databaseProvider);

        if (databaseProvider == nameof(EnumGestoresBD.SqlServer))
        {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString(databaseProvider),
                    sqlServerOptionsAction: sqlOptions => {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null
                    );
                });                
            });
        }
        else if (databaseProvider == nameof(EnumGestoresBD.PostgreSql))
        {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString(databaseProvider),
                    npgsqlOptionsAction: npgsqlOption => {
                        npgsqlOption.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null
                    );
                }); 
            });
        }
              
        return services;
    }
}