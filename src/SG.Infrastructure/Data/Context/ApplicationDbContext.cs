using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SG.Domain.Commun.Entities;
using SG.Shared.Enumerators;

namespace SG.Infrastructure.Data.Context;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _databaseProvider;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;     
        _databaseProvider = _configuration.GetSection("DatabaseProvider").Value!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_databaseProvider == nameof(EnumGestoresBD.SqlServer))
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString(_databaseProvider));
        }
        else if (_databaseProvider == nameof(EnumGestoresBD.PostgreSql))
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(_databaseProvider));
        }
        else
        {
            throw new Exception("Unsupported database provider");
        }               
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    // Registered DB Model
    public DbSet<Catalogue> Catalogue { get; set; }
}
