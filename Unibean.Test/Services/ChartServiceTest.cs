using Enable.EnumDisplayName;
using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Charts;
using Unibean.Service.Services;
using Type = System.Type;

namespace Unibean.Test.Services;

public class ChartServiceTest
{
    private readonly IAdminRepository adminRepository;

    private readonly IBrandRepository brandRepository;

    private readonly ICampaignRepository campaignRepository;

    private readonly IProductRepository productRepository;

    private readonly IStaffRepository staffRepository;

    private readonly IStoreRepository storeRepository;

    private readonly IOrderRepository orderRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IActivityRepository activityRepository;

    private readonly IVoucherItemRepository voucherItemRepository;

    private readonly IOrderTransactionRepository orderTransactionRepository;

    private readonly IActivityTransactionRepository activityTransactionRepository;

    private readonly IRequestTransactionRepository requestTransactionRepository;

    private readonly ICampaignTransactionRepository campaignTransactionRepository;

    private readonly IBonusTransactionRepository bonusTransactionRepository;

    public ChartServiceTest()
    {
        adminRepository = A.Fake<IAdminRepository>();
        brandRepository = A.Fake<IBrandRepository>();
        campaignRepository = A.Fake<ICampaignRepository>();
        productRepository = A.Fake<IProductRepository>();
        staffRepository = A.Fake<IStaffRepository>();
        storeRepository = A.Fake<IStoreRepository>();
        orderRepository = A.Fake<IOrderRepository>();
        studentRepository = A.Fake<IStudentRepository>();
        activityRepository = A.Fake<IActivityRepository>();
        voucherItemRepository = A.Fake<IVoucherItemRepository>();
        orderTransactionRepository = A.Fake<IOrderTransactionRepository>();
        activityTransactionRepository = A.Fake<IActivityTransactionRepository>();
        requestTransactionRepository = A.Fake<IRequestTransactionRepository>();
        campaignTransactionRepository = A.Fake<ICampaignTransactionRepository>();
        bonusTransactionRepository = A.Fake<IBonusTransactionRepository>();
    }

    [Fact]
    public void ChartService_GetColumnChart()
    {
        // Arrange
        string id = "id";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-3);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        Role role = Role.Admin;
        A.CallTo(() => studentRepository.CountStudentToday(A<DateOnly>.Ignored))
            .Returns(1);
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetColumnChart(id, fromDate, toDate, isAsc, role);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<ColumnChartModel>));
        Assert.Equal(4, result.Count);
        Assert.Equal(4, result.Sum(r => r.Value));
    }

    [Fact]
    public void ChartService_GetLineChart()
    {
        // Arrange
        string id = "id";
        Role role = Role.Admin;
        A.CallTo(() => activityTransactionRepository.IncomeOfGreenBean(A<DateOnly>.Ignored))
            .Returns(1);
        A.CallTo(() => orderTransactionRepository.IncomeOfRedBean(A<DateOnly>.Ignored))
            .Returns(2);
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetLineChart(id, role);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<LineChartModel>));
        Assert.Equal(7, result.Count);
        Assert.Equal(7, result.Sum(r => r.Green));
        Assert.Equal(14, result.Sum(r => r.Red));
    }

    [Fact]
    public void ChartService_GetRankingChart()
    {
        // Arrange
        string id = "id";
        Type type = typeof(Brand);
        Role role = Role.Admin;
        A.CallTo(() => adminRepository.GetById(id))
            .Returns(new());
        A.CallTo(() => brandRepository.GetRanking(10))
            .Returns(new()
            {
                new()
                {
                    BrandName = "name1",
                    Account = new()
                    {
                        Avatar = "avatar1"
                    },
                    TotalSpending = 100,
                },
                new()
                {
                    BrandName = "name2",
                    Account = new()
                    {
                        Avatar = "avatar2"
                    },
                    TotalSpending = 200,
                },
                new()
                {
                    BrandName = "name3",
                    Account = new()
                    {
                        Avatar = "avatar3"
                    },
                    TotalSpending = 200,
                },
            });
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetRankingChart(id, type, role);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<RankingModel>));
        Assert.Equal(3, result.Count);
        Assert.Equal(2, result.Max(r => r.Rank));
    }

    [Fact]
    public void ChartService_GetTitleAdmin()
    {
        // Arrange
        string adminId = "id";
        A.CallTo(() => brandRepository.CountBrand())
            .Returns(1);
        A.CallTo(() => campaignRepository.CountCampaign())
            .Returns(2);
        A.CallTo(() => productRepository.CountProduct())
            .Returns(3);
        A.CallTo(() => studentRepository.CountStudent())
            .Returns(4);
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetTitleAdmin(adminId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TitleAdminModel));
        Assert.Equal(1, result.NumberOfBrands);
        Assert.Equal(2, result.NumberOfCampagins);
        Assert.Equal(3, result.NumberOfProducts);
        Assert.Equal(4, result.NumberOfStudents);
    }

    [Fact]
    public void ChartService_GetTitleBrand()
    {
        // Arrange
        string brandId = "id";
        A.CallTo(() => brandRepository.GetById(brandId))
            .Returns(new()
            {
                Campaigns = new List<Campaign>()
                {
                    new()
                },
                Stores = new List<Store>()
                {
                    new(),
                    new()
                },
                Vouchers = new List<Voucher>()
                {
                    new()
                    {
                        VoucherItems = new List<VoucherItem>()
                        {
                            new(),
                            new(),
                            new()
                        }
                    }
                },
                Wallets = new List<Wallet>()
                {
                    new()
                    {
                        Balance = 4,
                        Type = WalletType.Green,
                    }
                }
            });
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetTitleBrand(brandId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TitleBrandModel));
        Assert.Equal(1, result.NumberOfCampagins);
        Assert.Equal(2, result.NumberOfStores);
        Assert.Equal(3, result.NumberOfVoucherItems);
        Assert.Equal(4, result.Balance);
    }

    [Fact]
    public void ChartService_GetTitleStaff()
    {
        // Arrange
        string staffId = "id";
        A.CallTo(() => staffRepository.GetById(staffId))
            .Returns(new()
            {
                Station = new()
                {
                    Orders = new List<Order>()
                    {
                        new()
                        {
                            Amount = 2
                        }
                    },
                    StationName = "stationName",
                    Image = "image",
                    State = StationState.Active,
                }
            });
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetTitleStaff(staffId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TitleStaffModel));
        Assert.Equal(1, result.NumberOfOrders);
        Assert.Equal(2, result.CostOfOrders);
        Assert.Equal("stationName", result.StationName);
        Assert.Equal("image", result.StationImage);
        Assert.Equal(1, result.StationStateId);
        Assert.Equal(StationState.Active.ToString(), result.StationState);
        Assert.Equal(StationState.Active.GetDisplayName(), result.StationStateName);
    }

    [Fact]
    public void ChartService_GetTitleStore()
    {
        // Arrange
        string storeId = "id";
        A.CallTo(() => storeRepository.GetById(storeId))
            .Returns(new()
            {
                Activities = new List<Activity>()
                {
                    new()
                },
                Bonuses = new List<Bonus>()
                {
                    new()
                    {
                        Amount = 1
                    },
                    new()
                    {
                        Amount = 2
                    }
                },
                Brand = new()
                {
                    Wallets = new List<Wallet>()
                    {
                        new()
                        {
                            Balance = 4,
                            Type = WalletType.Green,
                        }
                    }
                }
            });
        var service = new ChartService
            (adminRepository, brandRepository, campaignRepository,
            productRepository, staffRepository, storeRepository,
            orderRepository, studentRepository, activityRepository,
            voucherItemRepository, orderTransactionRepository, activityTransactionRepository,
            requestTransactionRepository, campaignTransactionRepository, bonusTransactionRepository);

        // Act
        var result = service.GetTitleStore(storeId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(TitleStoreModel));
        Assert.Equal(1, result.NumberOfParticipants);
        Assert.Equal(2, result.NumberOfBonuses);
        Assert.Equal(3, result.AmountOfBonuses);
        Assert.Equal(4, result.BrandBalance);
    }
}
