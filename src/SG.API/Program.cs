using Microsoft.EntityFrameworkCore;
using SG.API.Extensions;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Services;
using SG.Application;

var builder = WebApplication.CreateBuilder(args);


// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

// Configuración de los servicios (equivalente a ConfigureServices)
builder.Services.AddControllers();

// Agregar soporte para Swagger/OpenAPI
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure Services

//var healthChecksBuilder = builder.Services.AddHealthChecks()

builder.Services.AddDatabaseConfiguration(builder.Configuration)
                .AddInfrastructure()
                .AddServicesApplication()
                .AddAutoMapperConfiguration();

#endregion


builder.Services.AddSwaggerConfigurationOpenApi();


var app = builder.Build();

// Usar CORS
app.UseCors("AllowAllOrigins");

app.AddSwaggerConfigurationUI();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


await using var serviceScope = app.Services.CreateAsyncScope();
await using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
// var inMemoryOptions = serviceScope.ServiceProvider.GetOptions<InMemoryOptions>()

try {


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


app.Logger.LogInformation("----- Application started...");
await app.RunAsync();
