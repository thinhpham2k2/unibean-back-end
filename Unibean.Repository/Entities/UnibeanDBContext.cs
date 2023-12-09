using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Unibean.Repository.Entities;

public class UnibeanDBContext : DbContext
{
    public UnibeanDBContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        string connetionString = configuration.GetConnectionString("UnibeanDB");
        optionsBuilder.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
    }

    public virtual DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
