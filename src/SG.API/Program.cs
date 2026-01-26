using SG.API.Configuration;
using Serilog;
using FluentValidation;
using SG.API.Extensions;
using SG.Application;
using System.Text.Json.Serialization;

const string AllowOrigins = "AllowAllOrigins";

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowOrigins, builder =>
    {
        // builder.AllowAnyOrigin()
        //         .AllowAnyMethod()
        //         .AllowAnyHeader();

        builder.WithOrigins(["http://localhost:3000"])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition");                
    });
});



// Configuración de los servicios (equivalente a ConfigureServices)
builder.Services.AddControllers()
     .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
     

// Agregar soporte para Swagger/OpenAPI
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddHttpContextAccessor();

builder.Services.AddDependencyInjection(builder.Configuration)
                .AddOptionsSettings(builder.Configuration);


builder.Services.AddSwaggerConfigurationOpenApi();

await builder.Services.EnsureSeedData(builder.Configuration);



builder.ConfigureSerilogFromFile(builder.Configuration, builder.Environment);


builder.Services.AddSerilog(); 


builder.Services.AddExceptionHandler<SG.API.Middlewares.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();


app.UseExceptionHandler();

// 👇 This add the Authentication Middleware
app.UseAuthentication();
// 👇 This add the Authorization Middleware
app.UseAuthorization();


// // 👇 The routes / and /public allow anonymous requests
// app.MapGet("/", () => "Hello World!");
// app.MapGet("/public", () => "Public Hello World!")
// 	.AllowAnonymous();

// // 👇 The routes /private require authorized request
// app.MapGet("/private", () => "Private Hello World!")
// 	.RequireAuthorization();   

// Usar CORS
app.UseCors(AllowOrigins);



// custom jwt auth middleware
app.UseMiddleware<SG.API.Middlewares.AuthorizationHanlderMiddleware>();

// global error handler
//app.UseMiddleware<SG.API.Middlewares.HttpLoggingMiddleware>();


app.AddSwaggerConfigurationUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapHealthChecks("health");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();


app.Use(next => context => {
    context.Request.EnableBuffering();
    return next(context);
});

//await app.ExecuteInformationDataBase();

app.Logger.LogInformation("----- Application started...");
await app.RunAsync();

public partial class Program;