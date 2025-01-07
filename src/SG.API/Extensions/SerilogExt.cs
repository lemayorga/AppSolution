
using Serilog;
using Serilog.Sinks.MSSqlServer;
using SG.Shared.Enumerators;
using SG.Shared.Helpers;

namespace SG.API.Extensions;
    
public static class SerilogExt
{

    internal  static void UseSerilogRequestLogging_ (this WebApplication app, IConfigurationManager configuration)
    {
        bool applyLogs = configuration.GetValue<bool>("ApplyLog"); 

        if(applyLogs)
        { 
            app.UseSerilogRequestLogging();
        }
    }


    public static void Configure(WebApplicationBuilder builder, ConfigurationManager configuration)
    {
      /*  configuration.SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("logging.json", optional: false, reloadOnChange: true);*/

        AppConfiguration.AddFileConfiguration(configuration, Directory.GetCurrentDirectory(),"logging.json");


       builder.Host.UseSerilog((context, services, loggerConfiguration) =>
       {
            var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");  
            var connectionStrings =  configuration.GetValue<string>($"ConnectionStrings:{databaseProvider}"); 

            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            switch (databaseProvider)
            {
                case nameof(EnumGestoresBD.SqlServer):

                    IConfiguration sqlConfig = builder.Configuration.GetSection("Serilog:WriteTo:1:Args");
                    var sqlTableName = sqlConfig["sinkOptionsSection:tableName"];
                    var autoCreateSqlTable = bool.Parse(sqlConfig["sinkOptionsSection:autoCreateSqlTable"] ?? "false");
    
                     loggerConfiguration.WriteTo.MSSqlServer(
                        connectionString: connectionStrings,
                        sinkOptions: new MSSqlServerSinkOptions { TableName = sqlTableName ,AutoCreateSqlTable = autoCreateSqlTable }
                    );

                    break;
                case nameof(EnumGestoresBD.PostgreSql):

                    IConfiguration pgConfig = builder.Configuration.GetSection("Serilog:WriteTo:1:Args");
                    var pgTableName = pgConfig["tableName"];
                    var needAutoCreateTable = bool.Parse(pgConfig["needAutoCreateTable"] ?? "false");

                    loggerConfiguration.WriteTo.PostgreSQL(
                        connectionString: connectionStrings,
                        tableName: pgTableName,
                        needAutoCreateTable: needAutoCreateTable
                    );

                    break;
            }

            loggerConfiguration.Enrich.FromLogContext();
        });

    }
}