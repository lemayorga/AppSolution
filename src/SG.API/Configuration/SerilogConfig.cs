using Serilog;
using SG.Shared.Helpers;
using SG.Shared.Settings;
namespace SG.API.Configuration;

public static class SerilogConfig
{
    internal static void ConfigureSerilog(this WebApplicationBuilder builder, ConfigurationManager configuration)
    {

        var settings = new AppConfiguration(configuration).GetAppSettings();

        if(!settings.EnableLoggingSerilog) 
        { 
            return; 
        }

        var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile(NamesFileApplicationSettings.SerilogConfig).Build())
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