using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Seeders;
using SG.Infrastructure.DatabaseFlavor;
using SG.Shared.Enumerators;
using Microsoft.EntityFrameworkCore;

namespace SG.API.Extensions;

internal  static  class DbContextExtensions
{
    internal static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
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
             
        services.AddDbContext<ApplicationDbContext>(optionsAction);

        return services;
    }


    public static async Task ExecuteInformation(WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try 
        {
            // https://github.com/jeangatto/ASP.NET-Core-API-DDD-SOLID/blob/main/src/SGP.PublicApi/Program.cs
            var connectionString = context.Database.GetConnectionString();
            app.Logger.LogInformation("----- Database Server: {Connection}", connectionString);
            app.Logger.LogInformation("----- Database Server: Checking for pending migrations...");

            /*if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                app.Logger.LogInformation("----- Database Server: Creating and migrating the database...");

            // await context.Database.MigrateAsync()

                app.Logger.LogInformation("----- Database Server: Database created and migrated successfully!");
            }
            else
            {
                app.Logger.LogInformation("----- Database Server: Migrations are up to date");
            }  */

            //app.Logger.LogInformation("----- Populating data base...")
            //await context.EnsureSeedDataAsync()

            app.Logger.LogInformation("----- Database populated successfully!");          
        }
        catch (Exception exception)
        {
            app.Logger.LogError(exception, "An exception occurred while starting the application: {Message}", exception.Message);
            throw ;
        }
    }
    
    public static async Task ExecuteSeeders(WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await SeederExecute.SeedAsync(context,app.Configuration);
    }
}