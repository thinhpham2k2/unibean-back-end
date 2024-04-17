using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Unibean.Repository.Entities;

public class UnibeanDBContext : DbContext
{
    public UnibeanDBContext()
    {
    }

    public UnibeanDBContext(DbContextOptions<UnibeanDBContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            string connetionString = configuration.GetConnectionString("UnibeanDB");
            optionsBuilder.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }
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
    public virtual DbSet<CampaignActivity> CampaignActivities { get; set; }
    public virtual DbSet<CampaignCampus> CampaignCampuses { get; set; }
    public virtual DbSet<CampaignDetail> CampaignDetails { get; set; }
    public virtual DbSet<CampaignMajor> CampaignMajors { get; set; }
    public virtual DbSet<CampaignStore> CampaignStores { get; set; }
    public virtual DbSet<CampaignTransaction> CampaignTransactions { get; set; }
    public virtual DbSet<CampaignType> CampaignTypes { get; set; }
    public virtual DbSet<Campus> Campuses { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Challenge> Challenges { get; set; }
    public virtual DbSet<ChallengeTransaction> ChallengeTransactions { get; set; }
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
    public virtual DbSet<Staff> Staffs { get; set; }
    public virtual DbSet<Station> Stations { get; set; }
    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<StudentChallenge> StudentChallenges { get; set; }
    public virtual DbSet<University> Universities { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<VoucherItem> VoucherItems { get; set; }
    public virtual DbSet<VoucherType> VoucherTypes { get; set; }
    public virtual DbSet<Wallet> Wallets { get; set; }
    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<Challenge> challenges = new()
        {
            new Challenge
            {
                Id = Ulid.NewUlid().ToString(),
                Type = ChallengeType.Verify,
                ChallengeName = "Xác nhận tài khoản sinh viên",
                Amount = 100000,
                Condition = 1,
                Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYHZ6MVH2JKG32NX15K3CDG?alt=media&token=17dfbee0-8324-4dcd-9f73-320436766c5",
                FileName = "01HKYHZ6MVH2JKG32NX15K3CDG",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Description = "Tài khoản sinh viên đã được xác thực sẽ hoàn thành thử thách",
                State = true,
                Status = true,

            }
        };

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Welcome,
            ChallengeName = "Đồng cam cộng hưởng",
            Amount = 50000,
            Condition = 1,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJ4PDDV2QZJCFZDDEVQ0PJ?alt=media&token=7397417a-2a73-47af-968a-9197cceaf43c",
            FileName = "01HKYJ4PDDV2QZJCFZDDEVQ0PJ",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Nhập mã giới thiệu của bạn bè để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 1",
            Amount = 10000,
            Condition = 1,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 1 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 10",
            Amount = 100000,
            Condition = 10,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 10 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 30",
            Amount = 300000,
            Condition = 30,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 30 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 50",
            Amount = 500000,
            Condition = 50,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 50 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 100",
            Amount = 1000000,
            Condition = 100,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 100 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 1000",
            Amount = 10000000,
            Condition = 1000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 1000 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Spread,
            ChallengeName = "Mời bạn cùng vui 10000",
            Amount = 100000000,
            Condition = 10000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40",
            FileName = "01HKYJFX60DHMR6FRY4532VSX7",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Được 10000 tài khoản nhập mã giới thiệu để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 1",
            Amount = 1000,
            Condition = 10000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 10000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 2",
            Amount = 10000,
            Condition = 100000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 100000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 3",
            Amount = 50000,
            Condition = 500000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 500000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 4",
            Amount = 100000,
            Condition = 1000000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 1000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 5",
            Amount = 1000000,
            Condition = 10000000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 10000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 6",
            Amount = 5000000,
            Condition = 50000000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 50000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        Thread.Sleep(1);
        challenges.Add(new Challenge
        {
            Id = Ulid.NewUlid().ToString(),
            Type = ChallengeType.Consume,
            ChallengeName = "Tiền tiêu như nước 7",
            Amount = 10000000,
            Condition = 100000000,
            Image = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c",
            FileName = "01HNM20GGS9Q5QPJQF4N29Y10F.png",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = "Tích trữ tiêu thụ 100000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách",
            State = true,
            Status = true,
        });

        modelBuilder.Entity<Challenge>().HasData(challenges);
    }
}
