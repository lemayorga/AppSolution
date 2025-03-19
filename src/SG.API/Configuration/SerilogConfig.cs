using Serilog;
namespace SG.API.Configuration;

public static class SerilogConfig
{
    internal static void ConfigureSerilog(this WebApplicationBuilder builder, ConfigurationManager configuration)
    {
            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("serilogconfig.json").Build())
                    .Enrich.FromLogContext()
                    .CreateLogger();

            Log.Logger = logger;
            builder.Logging.AddSerilog(logger);
            builder.Services.AddSerilog(logger);
            builder.Host.UseSerilog();
    }
}