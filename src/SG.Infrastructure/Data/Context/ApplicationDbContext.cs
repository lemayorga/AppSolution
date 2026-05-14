using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using SG.Domain.Entities.Commun;
using SG.Domain.Entities.Security;
using SG.Infrastructure.Data.Config;
using SG.Shared.Enumerators;
using SG.Shared.Settings;
using Action = SG.Domain.Entities.Security.Action;
using Module = SG.Domain.Entities.Security.Module;

namespace SG.Infrastructure.Data.Context;

/// <summary>
/// Database context for the application, using Entity Framework Core.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Constructor for the database context, which receives the configuration options, the application's general configuration, the specific settings, and the database configuration.
    /// </summary>
    private readonly IConfiguration _configuration;
    
    /// <summary>
    /// Application settings, which may include various configuration options for the application, such as logging, connection strings, etc.
    /// </summary>
    private readonly AppSettings _settings;

    /// <summary>
    /// Database configuration, which includes details about the database provider, connection string, and other related settings.
    /// </summary>
    private readonly IDatabaseConfiguration  _dbConfig;
    
    /// <summary>
    /// Constructor for the database context, which receives the configuration options, the application's general configuration, the specific settings, and the database configuration.
    /// </summary>
    /// <param name="options">The configuration options for the database context.</param>
    /// <param name="configuration">The application's general configuration.</param>
    /// <param name="settings">The specific settings for the application.</param>
    /// <param name="dbConfig">The database configuration.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, IOptions<AppSettings> settings, IDatabaseConfiguration dbConfig) : base(options)
    {
        _configuration = configuration;     
        _settings =  settings.Value;
       _dbConfig = dbConfig;
    }

    /// <summary>
    /// This method checks the database provider specified in the configuration and returns an appropriate connection object (e.g., SqlConnection for SQL Server, NpgsqlConnection for PostgreSQL). If the database provider is not supported, it throws a NotImplementedException.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IDbConnection CreateConnection()
    {
      string connectionString = _dbConfig.ConnectionString;
      return _dbConfig.DatabaseProvider switch
        {
            EnumGestoresBD.SqlServer =>  new SqlConnection(connectionString),
            EnumGestoresBD.PostgreSql => new NpgsqlConnection(connectionString),
            _ => throw new NotImplementedException(),
        };
    }
    
    /// <summary>
    /// This method is responsible for configuring the database context. It checks if logging for Entity Framework Core is enabled in the application settings, and if so, it configures the logging to output to the console. Additionally, it checks if sensitive data logging is enabled and configures that as well. This allows developers to see detailed logs of database operations, which can be useful for debugging and monitoring purposes.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if(_settings.EnableLoggingEntityFrameworkCore)
        {
            bool enableSensitiveDataLoggingEF =  _settings.EnableSensitiveDataLoggingEntityFrameworkCore;
            
            optionsBuilder
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging(enableSensitiveDataLoggingEF);
        }
    }
    
    /// <summary>
    /// This method is responsible for configuring the model for the database context. It applies all configurations from the assembly where the context is defined, which allows for a clean separation of entity configurations into separate classes. This method is called when the model for the context is being created, and it ensures that all entity configurations are applied correctly to the model.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    #region Commun
    public DbSet<Catalogue> Catalogue { get; set; }
    #endregion

    #region Security
    public DbSet<PasswordHistory> PasswordHistory { get; set; }
    public DbSet<PasswordPolicy> PasswordPolicy { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UsersRoles> UsersRoles { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Permission> Permissions { get; set; }    
    public DbSet<UsersToken> UsersToken { get; set; }    
    #endregion
}
