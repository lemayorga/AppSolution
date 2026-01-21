using Serilog;
using SG.Shared.Helpers;
namespace SG.API.Configuration;

public static class SerilogConfig
{
    internal static void ConfigureSerilogFromFile(this WebApplicationBuilder builder, ConfigurationManager configuration,IWebHostEnvironment env)
    {
        var settings = new AppConfiguration(configuration).GetAppSettings();

        if(!settings.EnableLoggingSerilog){  return; }
    
        bool isDevelopment = env.IsDevelopment();
        string fileNameSerilogConfig = $"serilogconfig{(isDevelopment ? ".Development" : "")}.json";

        var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                         .AddJsonFile(fileNameSerilogConfig)
                         .Build())
                .Enrich.FromLogContext();

        if(settings.EnableLoggingEntityFrameworkCore)
        {
           loggerConfig.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Information);
        }

        var logger =  loggerConfig.CreateLogger();
        Log.Logger = logger;

        builder.Logging.AddSerilog(logger);
        builder.Services.AddSerilog(logger);
        builder.Host.UseSerilog();


        builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
    }
}