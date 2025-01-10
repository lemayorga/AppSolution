using SG.API.Extensions;
using SG.API.Configuration;

const string AllowOrigins = "AllowAllOrigins";

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowOrigins, builder =>
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


builder.Services.AddHttpContextAccessor();

builder.Services.AddDependencyInjection(builder.Configuration);



builder.Services.AddSwaggerConfigurationOpenApi();

await builder.Services.EnsureSeedData(builder.Configuration);

builder.ConfigureSerilog(builder.Configuration);

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

app.IfUseSerilogRequestLogging(builder.Configuration);

// Usar CORS
app.UseCors(AllowOrigins);

app.AddSwaggerConfigurationUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapHealthChecks("health");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.ExecuteInformationDataBase();

app.Logger.LogInformation("----- Application started...");
await app.RunAsync();
