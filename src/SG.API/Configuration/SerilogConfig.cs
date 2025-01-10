using Serilog;
using Serilog.Sinks.MSSqlServer;
using SG.Infrastructure.Data.Config;
using SG.Shared.Enumerators;
using SG.Shared.Helpers;

namespace SG.API.Configuration;

public static class SerilogConfig
{
    internal  static void IfUseSerilogRequestLogging (this WebApplication app, IConfigurationManager configuration)
    {
        bool applyLogs = configuration.GetValue<bool>("ApplyLog"); 

        if(applyLogs)
        { 
            app.UseSerilogRequestLogging();
        }
    }

    internal static void ConfigureSerilog(this WebApplicationBuilder builder, ConfigurationManager configuration)
    {
      /*  configuration.SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("logging.json", optional: false, reloadOnChange: true);*/

        AppConfiguration.AddFileConfiguration(configuration, Directory.GetCurrentDirectory(),"logging.json");

       builder.Host.UseSerilog((context, serviceProvider, loggerConfiguration) =>
       {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var configDataBase = scope.ServiceProvider.GetRequiredService<IDatabaseConfiguration>();

            loggerConfiguration.ReadFrom.Configuration(context.Configuration);

            IConfiguration configFile = builder.Configuration.GetSection("Serilog:WriteTo:1:Args");  
            switch (configDataBase.DatabaseProvider)
            {
                case EnumGestoresBD.SqlServer:

                    loggerConfiguration.WriteTo.MSSqlServer
                    (
                        connectionString: configDataBase.ConnectionString,
                        sinkOptions: new MSSqlServerSinkOptions 
                        { 
                            TableName =  configFile["sinkOptionsSection:tableName"],
                             AutoCreateSqlTable = bool.Parse(configFile["sinkOptionsSection:autoCreateSqlTable"] ?? "false") 
                        }
                    );

                    break;
                case EnumGestoresBD.PostgreSql:

                    loggerConfiguration.WriteTo.PostgreSQL
                    (
                        connectionString: configDataBase.ConnectionString,
                        tableName:  configFile["tableName"],
                        needAutoCreateTable: bool.Parse(configFile["needAutoCreateTable"] ?? "false")
                    );

                    break;
            }

            loggerConfiguration.Enrich.FromLogContext();
        });

    }
}
