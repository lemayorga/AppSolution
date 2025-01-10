using Microsoft.EntityFrameworkCore;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Seeders;

namespace SG.API.Configuration;

internal static class DataBaseConfig
{
    internal static async Task EnsureSeedData(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var context  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await EnsureSeedData(env, configuration, context);
    }

    static async Task EnsureSeedData(IWebHostEnvironment env,IConfiguration configuration, ApplicationDbContext context)
    {
        if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            await context.Database.EnsureCreatedAsync();

        if (!env.IsDevelopment() || env.EnvironmentName.Equals("Docker"))//Sólo en ambiente development o docker
        {
             if ((await context.Database.GetPendingMigrationsAsync()).Any())//Sólo cuando haya migraciones pendientes
            {
                await context.Database.MigrateAsync();
            }
        }
         await SeederExecute.SeedAsync(context, configuration);
    }
}
