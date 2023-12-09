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

    public virtual DbSet<Area> Areas { get; set; }
    public virtual DbSet<Campus> Campuses { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<OrderState> OrderStates { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<Station> Stations { get; set; }
    public virtual DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
