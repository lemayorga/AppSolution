using Serilog;
using SG.Shared.Settings;
namespace SG.API.Configuration;

public static class SerilogConfig
{
    internal static void ConfigureSerilog(this WebApplicationBuilder builder, ConfigurationManager configuration)
    {

        AppSettings settings = new();
        configuration.GetSection(NamesApplicationSettings.ApplicationSettings).Bind(settings);

        if(!settings.EnableLoggingSerilog) {return; }

        var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("serilogconfig.json").Build())
                .Enrich.FromLogContext()
                .CreateLogger();

        Log.Logger = logger;

        builder.Logging.AddSerilog(logger);
        builder.Services.AddSerilog(logger);
        builder.Host.UseSerilog();


        builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
    }
}