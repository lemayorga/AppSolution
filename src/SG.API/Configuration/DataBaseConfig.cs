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
            await using var context  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await EnsureSeedData(env, configuration, context);
        }
        catch(Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    static async Task EnsureSeedData(IWebHostEnvironment env,IConfiguration configuration, ApplicationDbContext context)
    {
        //await context.Database.EnsureCreatedAsync();

        if (!env.IsDevelopment())
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
        await SeederExecute.SeedAsync(context, configuration);
    }
}
