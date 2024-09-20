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
        var databaseConexion = _configuration.GetConnectionString(_databaseProvider);
        _ = _databaseProvider switch 
        {
             nameof(EnumGestoresBD.SqlServer) => optionsBuilder.UseSqlServer(databaseConexion),
             nameof(EnumGestoresBD.PostgreSql) => optionsBuilder.UseNpgsql(databaseConexion),
             _ => throw new NotImplementedException("Unsupported database provider"),
        };

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
