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

    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Activity> Activities { get; set; }
    public virtual DbSet<ActivityTransaction> ActivityTransactions { get; set; }
    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<Area> Areas { get; set; }
    public virtual DbSet<Bonus> Bonuses { get; set; }
    public virtual DbSet<BonusTransaction> BonusTransactions { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Campaign> Campaigns { get; set; }
    public virtual DbSet<CampaignCampus> CampaignCampuses { get; set; }
    public virtual DbSet<CampaignMajor> CampaignMajors { get; set; }
    public virtual DbSet<CampaignStore> CampaignStores { get; set; }
    public virtual DbSet<CampaignType> CampaignTypes { get; set; }
    public virtual DbSet<Campus> Campuses { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Challenge> Challenges { get; set; }
    public virtual DbSet<ChallengeTransaction> ChallengeTransactions { get; set; }
    public virtual DbSet<ChallengeType> ChallengeTypes{ get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Invitation> Invitations { get; set; }
    public virtual DbSet<Major> Majors { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<OrderState> OrderStates { get; set; }
    public virtual DbSet<OrderTransaction> OrderTransactions { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Request> Requests { get; set; }
    public virtual DbSet<RequestTransaction> RequestTransactions { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<Station> Stations { get; set; }
    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<StudentChallenge> StudentChallenges { get; set; }
    public virtual DbSet<University> Universities { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<VoucherItem> VoucherItems { get; set; }
    public virtual DbSet<VoucherType> VoucherTypes { get; set; }
    public virtual DbSet<Wallet> Wallets { get; set; }
    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }
    public virtual DbSet<WalletType> WalletTypes { get; set; }
    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
