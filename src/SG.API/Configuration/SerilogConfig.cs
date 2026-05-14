using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using SG.Shared.Enumerators;
using SG.Shared.Helpers;
using SG.Shared.Settings;
using Serilog.Exceptions;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.File;

namespace SG.API.Configuration;

public static class SerilogConfig
{
    internal static void ConfigureSerilogFromFile(this WebApplicationBuilder builder, ConfigurationManager configuration, IWebHostEnvironment env)
    {
        var settings = new AppConfiguration(configuration).GetAppSettings();

        if(!settings.EnableLoggingSerilog){  return; }

        var loggerConfig = GenerateConfigurationLogger(configuration, settings);
        var logger =  loggerConfig.CreateLogger();

        Log.Logger = logger;

        builder.Logging.AddSerilog(logger);
        builder.Services.AddSerilog(logger);
        builder.Host.UseSerilog(logger);

        builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
    }


    internal static LoggerConfiguration GenerateConfigurationLogger(ConfigurationManager configuration,AppSettings settings)
    {
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Verbose()               
            .MinimumLevel.Debug()                
            .MinimumLevel.Information()         
            .MinimumLevel.Warning()              
            .MinimumLevel.Error()                 
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)  
            .Enrich.FromLogContext() 
            .Enrich.WithMachineName() 
            .Enrich.WithThreadId()   
            .Enrich.WithProperty("ApplicationName", "MiApiDemo") 
            .Enrich.FromLogContext() 
            .Enrich.WithEnvironmentUserName() 
            .Enrich.WithExceptionDetails()  
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}");


        if(settings.EnableLoggingEntityFrameworkCore)
        {
            loggerConfig.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Information);
        }

        if(settings.EnableLoggingSerilogFile)
        {
            loggerConfig.WriteTo.File(path: "logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Username} {Message:lj}{NewLine}{Exception}", shared: true);
        }

        var databaseProvider = configuration.GetValue<string>(NamesApplicationSettings.DatabaseProvider);      
        var connectionStrings = configuration.GetValue<string>($"ConnectionStrings:{databaseProvider}"); 

        string tableName = "logs";

        if (databaseProvider ==  nameof(EnumGestoresBD.PostgreSql))
        {
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "raise_date", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
                { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                { "props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) }
                //{ "machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
            };   

            loggerConfig.WriteTo.PostgreSQL(connectionStrings!, tableName, columnWriters, needAutoCreateTable: true, schemaName: "public", 
                                            useCopy: true, queueLimit: 3000, batchSizeLimit: 40, period: new TimeSpan(0, 0, 10), formatProvider: null);
        }
        else if (databaseProvider == nameof(EnumGestoresBD.SqlServer))
        {
            loggerConfig.WriteTo.MSSqlServer(connectionString: connectionStrings!,sinkOptions:  new MSSqlServerSinkOptions { TableName = tableName, SchemaName = "dbo", AutoCreateSqlTable = true });

        }

        return loggerConfig;
    }
}