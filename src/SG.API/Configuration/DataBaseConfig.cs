using Microsoft.EntityFrameworkCore;
using Serilog;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Seeders;

namespace SG.API.Configuration;

internal static class DataBaseConfig
{
    internal static async Task EnsureSeedData(this IServiceCollection services, IConfiguration configuration)
    {
        try 
        {
            var serviceProvider = services.BuildServiceProvider();
            await using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var context  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

          //  await EnsureSeedData(env, configuration, context);
        }
        catch(Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    static async Task EnsureSeedData(IWebHostEnvironment env,IConfiguration configuration, ApplicationDbContext context)
    {
      //  if (env.IsDevelopment())
        await context.Database.EnsureCreatedAsync();

        if (!env.IsDevelopment())//Sólo en ambiente development o docker
        {
             if ((await context.Database.GetPendingMigrationsAsync()).Any())//Sólo cuando haya migraciones pendientes
            {
                await context.Database.MigrateAsync();
            }
        }
         await SeederExecute.SeedAsync(context, configuration);
    }
}
