using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SG.Domain.Commun.Entities;
using SG.Domain.Security.Entities;
using Action = SG.Domain.Security.Entities.Action;
using Module = SG.Domain.Security.Entities.Module;

namespace SG.Infrastructure.Data.Context;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;     
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
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
    public DbSet<Action> Actions { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Permission> Permissions { get; set; }    
    #endregion
}
