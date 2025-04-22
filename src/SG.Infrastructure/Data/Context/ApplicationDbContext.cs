using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SG.Domain.Commun.Entities;
using SG.Domain.Security.Entities;
using SG.Shared.Settings;
using Action = SG.Domain.Security.Entities.Action;
using Module = SG.Domain.Security.Entities.Module;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if(_settings.EnableLoggingEntityFrameworkCore)
        {
            optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();
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
