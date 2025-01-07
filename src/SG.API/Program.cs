using SG.API.Extensions;
using SG.Infrastructure.Services;
using SG.Application;

var builder = WebApplication.CreateBuilder(args);

SerilogExt.Configure(builder,builder.Configuration);


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



app.UseSerilogRequestLogging_(builder.Configuration);

// Usar CORS
app.UseCors("AllowAllOrigins");

app.AddSwaggerConfigurationUI();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DbContextExtensions.ExecuteInformation(app);
await DbContextExtensions.ExecuteSeeders(app);

app.Logger.LogInformation("----- Application started...");
await app.RunAsync();
