using SG.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace SG.API.Extensions;

internal  static  class DbContextExtensions
{

    internal static async Task ExecuteInformationDataBase(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try 
        {
            // https://github.com/jeangatto/ASP.NET-Core-API-DDD-SOLID/blob/main/src/SGP.PublicApi/Program.cs
            var connectionString = context.Database.GetConnectionString();
            app.Logger.LogDebug("----- Database Server: {Connection}", connectionString);
            app.Logger.LogDebug("----- Database Server: Checking for pending migrations...");

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                app.Logger.LogDebug("----- Database Server: Creating and migrating the database...");

            // await context.Database.MigrateAsync()

              //  app.Logger.LogInformation("----- Database Server: Database created and migrated successfully!");
            }
            else
            {
                app.Logger.LogDebug("----- Database Server: Migrations are up to date");
            }  

            //app.Logger.LogInformation("----- Populating data base...")
            //await context.EnsureSeedDataAsync()

            app.Logger.LogDebug("----- Database populated successfully!");          
        }
        catch (Exception exception)
        {
            app.Logger.LogError(exception, "An exception occurred while starting the application: {Message}", exception.Message);
            throw ;
        }
    }
}