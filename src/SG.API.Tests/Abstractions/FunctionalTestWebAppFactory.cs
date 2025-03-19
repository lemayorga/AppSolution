using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SG.Infrastructure.Data.Config;
using SG.Infrastructure.Data.Context;
using SG.Shared.Enumerators;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;
namespace SG.API.Tests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:bullseye")
        .WithDatabase("productsTemporal")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
   // private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((context, conf) =>
        {
             conf
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false);
        })
        .ConfigureTestServices(services => 
        {
            var descriptor  = services.SingleOrDefault(s => s.ServiceType  == typeof(DbContextOptions<ApplicationDbContext>));
            if(descriptor is not null)
            {
                services.Remove(descriptor);
            }

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var databaseConfiguration = scope.ServiceProvider.GetRequiredService<IDatabaseConfiguration>();
 
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                _  =  databaseConfiguration.DatabaseProvider  switch
                {
                    EnumGestoresBD.SqlServer => opt.UseSqlServer(_dbContainer.GetConnectionString()),
                    EnumGestoresBD.PostgreSql => opt.UseNpgsql(_dbContainer.GetConnectionString()),
                    _ => throw new NotImplementedException(),
                };
            });         
        });
    }
    
    public async Task InitializeAsync()
    {
       //return _dbContainer.StartAsync(); 
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
       return _dbContainer.StopAsync();
    }
}



// https://www.youtube.com/watch?v=ASa8wXMXwrQ