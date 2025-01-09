using SG.API.Extensions;
using SG.Infrastructure.Services;
using SG.Application;
using SG.Infrastructure.Auth;
using SG.Infrastructure.Auth.JwtAuthentication;

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
builder.Services.AddHttpContextAccessor();
builder.Services.AddDatabaseConfiguration(builder.Configuration)
                .AddInfrastructure()
                .AddServicesApplication()
                .AddAutoMapperConfiguration()
                .AddInfrastructureAuth(builder.Configuration);

#endregion


builder.Services.AddSwaggerConfigurationOpenApi();



var app = builder.Build();


// 👇 This add the Authentication Middleware
app.UseAuthentication();
// 👇 This add the Authorization Middleware
app.UseAuthorization();


// 👇 The routes / and /public allow anonymous requests
app.MapGet("/", () => "Hello World!");
app.MapGet("/public", () => "Public Hello World!")
	.AllowAnonymous();

// 👇 The routes /private require authorized request
app.MapGet("/private", () => "Private Hello World!")
	.RequireAuthorization();   

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
