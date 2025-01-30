using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace SG.API.Tests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("runtrackr")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }
    
    public Task InitializeAsync()
    {
       return _dbContainer.StartAsync();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
       return _dbContainer.StopAsync();
    }
}



// https://www.youtube.com/watch?v=ASa8wXMXwrQ