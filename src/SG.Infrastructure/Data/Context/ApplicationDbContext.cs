using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SG.Domain.Entities.Commun;
using SG.Domain.Entities.Security;
using SG.Shared.Settings;
using Action = SG.Domain.Entities.Security.Action;
using Module = SG.Domain.Entities.Security.Module;

namespace SG.Infrastructure.Data.Context;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly AppSettings _settings;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, IOptions<AppSettings> settings) : base(options)
    {
        _configuration = configuration;     
        _settings =  settings.Value;
    }

    public IDbConnection CreateConnection(string connectionString = "DefaultConnection")
    {
        string? connection = _configuration.GetConnectionString(connectionString);
        return  new SqlConnection(connectionString);
    }
    
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
