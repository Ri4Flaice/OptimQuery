using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using OptimQuery.Data.Entities;

namespace OptimQuery.Data.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext()
    {
    }
    
    public DbSet<UserEntity> UserEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        if (optionsBuilder.IsConfigured) return;
        
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.Local.json");

        var config = builder.Build();

        optionsBuilder.UseNpgsql(config.GetConnectionString("OptimQueryDB"));
    }
}