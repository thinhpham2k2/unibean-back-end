using AutoMapper;
using Enable.EnumDisplayName;
using MoreLinq;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Charts;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Type = System.Type;

namespace Unibean.Service.Services;

public class ChartService : IChartService
{
    private readonly Mapper mapper;

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

    public ChartService(
        IAdminRepository adminRepository,
        IBrandRepository brandRepository,
        ICampaignRepository campaignRepository,
        IProductRepository productRepository,
        IStaffRepository staffRepository,
        IStoreRepository storeRepository,
        IOrderRepository orderRepository,
        IStudentRepository studentRepository,
        IActivityRepository activityRepository,
        IVoucherItemRepository voucherItemRepository,
        IOrderTransactionRepository orderTransactionRepository,
        IActivityTransactionRepository activityTransactionRepository,
        IRequestTransactionRepository requestTransactionRepository,
        ICampaignTransactionRepository campaignTransactionRepository,
        IBonusTransactionRepository bonusTransactionRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Brand, TitleBrandModel>()
            .ForMember(t => t.NumberOfCampagins, opt => opt.MapFrom(src => src.Campaigns.Count))
            .ForMember(t => t.NumberOfStores, opt => opt.MapFrom(src => src.Stores.Count))
            .ForMember(t => t.NumberOfVoucherItems, opt => opt.MapFrom(
                src => src.Vouchers.Select(v => v.VoucherItems.Count).Sum()))
            .ForMember(t => t.Balance, opt => opt.MapFrom(
                src => src.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Balance))
            .ReverseMap();
            cfg.CreateMap<Staff, TitleStaffModel>()
            .ForMember(t => t.NumberOfOrders, opt => opt.MapFrom(src => src.Station.Orders.Count))
            .ForMember(t => t.CostOfOrders, opt => opt.MapFrom(src => src.Station.Orders.Select(o => o.Amount).Sum()))
            .ForMember(t => t.StationName, opt => opt.MapFrom(src => src.Station.StationName))
            .ForMember(t => t.StationImage, opt => opt.MapFrom(src => src.Station.Image))
            .ForMember(t => t.StationStateId, opt => opt.MapFrom(src => src.Station.State))
            .ForMember(t => t.StationState, opt => opt.MapFrom(src => src.Station.State))
            .ForMember(t => t.StationStateName, opt => opt.MapFrom(src => src.Station.State.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<Store, TitleStoreModel>()
            .ForMember(t => t.NumberOfParticipants, opt => opt.MapFrom(src => src.Activities.Select(a => a.StudentId).Distinct().Count()))
            .ForMember(t => t.NumberOfBonuses, opt => opt.MapFrom(src => src.Bonuses.Count))
            .ForMember(t => t.AmountOfBonuses, opt => opt.MapFrom(src => src.Bonuses.Select(b => b.Amount).Sum()))
            .ForMember(t => t.BrandBalance, opt => opt.MapFrom(
                src => src.Brand.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Balance))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.adminRepository = adminRepository;
        this.brandRepository = brandRepository;
        this.campaignRepository = campaignRepository;
        this.productRepository = productRepository;
        this.staffRepository = staffRepository;
        this.storeRepository = storeRepository;
        this.orderRepository = orderRepository;
        this.studentRepository = studentRepository;
        this.activityRepository = activityRepository;
        this.voucherItemRepository = voucherItemRepository;
        this.orderTransactionRepository = orderTransactionRepository;
        this.activityTransactionRepository = activityTransactionRepository;
        this.requestTransactionRepository = requestTransactionRepository;
        this.campaignTransactionRepository = campaignTransactionRepository;
        this.bonusTransactionRepository = bonusTransactionRepository;
    }

    public List<ColumnChartModel> GetColumnChart
        (string id, DateOnly fromDate, DateOnly toDate, bool? isAsc, Role role)
    {
        if (toDate < fromDate)
        {
            throw new InvalidParameterException("Ngày không hợp lệ");
        }
        List<ColumnChartModel> result = new();
        switch (role)
        {
            case Role.Admin:
                Admin admin = adminRepository.GetById(id);
                if (admin != null)
                {
                    while (toDate >= fromDate)
                    {
                        result.Add(new()
                        {
                            // Tổng tài khoản sinh viên được đăng kí trong ngày
                            Value = studentRepository.CountStudentToday(fromDate),

                            // Ngày diễn ra
                            Date = fromDate,
                        });
                        fromDate = fromDate.AddDays(1); // Tăng ngày hiện tại lên 1 ngày
                    }
                    return isAsc == null ?
                        result : (bool)isAsc ?
                        result.OrderBy(r => r.Value).ToList() : result.OrderByDescending(r => r.Value).ToList();
                }
                throw new InvalidParameterException("Không tìm thấy quản trị viên");
            case Role.Brand:
                Brand brand = brandRepository.GetById(id);
                if (brand != null)
                {
                    while (toDate >= fromDate)
                    {
                        result.Add(new()
                        {
                            // Tổng số lượng khuyến mãi được sử dụng trong ngày
                            Value = voucherItemRepository.CountVoucherItemToday(brand.Id, fromDate),

                            // Ngày diễn ra
                            Date = fromDate,
                        });
                        fromDate = fromDate.AddDays(1); // Tăng ngày hiện tại lên 1 ngày
                    }
                    return isAsc == null ?
                        result : (bool)isAsc ?
                        result.OrderBy(r => r.Value).ToList() : result.OrderByDescending(r => r.Value).ToList();
                }
                throw new InvalidParameterException("Không tìm thấy thương hiệu");
            case Role.Staff:
                Staff staff = staffRepository.GetById(id);
                if (staff != null)
                {
                    while (toDate >= fromDate)
                    {
                        result.Add(new()
                        {
                            // Tổng số lượng đơn hàng được tạo trong ngày
                            Value = orderRepository.CountOrderToday(staff.StationId, fromDate),

                            // Ngày diễn ra
                            Date = fromDate,
                        });
                        fromDate = fromDate.AddDays(1); // Tăng ngày hiện tại lên 1 ngày
                    }
                    return isAsc == null ?
                        result : (bool)isAsc ?
                        result.OrderBy(r => r.Value).ToList() : result.OrderByDescending(r => r.Value).ToList();
                }
                throw new InvalidParameterException("Không tìm thấy nhân viên");
            case Role.Store:
                Store store = storeRepository.GetById(id);
                if (store != null)
                {
                    while (toDate >= fromDate)
                    {
                        result.Add(new()
                        {
                            // Tổng số lượng người tham gia trong ngày
                            Value = activityRepository.CountParticipantToday(store.Id, fromDate),

                            // Ngày diễn ra
                            Date = fromDate,
                        });
                        fromDate = fromDate.AddDays(1); // Tăng ngày hiện tại lên 1 ngày
                    }
                    return isAsc == null ?
                        result : (bool)isAsc ?
                        result.OrderBy(r => r.Value).ToList() : result.OrderByDescending(r => r.Value).ToList();
                }
                throw new InvalidParameterException("Không tìm thấy cửa hàng");
            default:
                throw new InvalidParameterException("Xác thực không hợp lệ");
        }
    }

    public List<LineChartModel> GetLineChart(string id, Role role)
    {
        List<LineChartModel> result = new();
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        switch (role)
        {
            case Role.Admin:
                Admin admin = adminRepository.GetById(id);
                if (admin != null)
                {
                    for (DateOnly d = date.AddDays(-6); d <= date; d = d.AddDays(1))
                    {
                        result.Add(new()
                        {
                            // Tổng đậu xanh thu được từ việc Student mua Voucher Item
                            Green = activityTransactionRepository.IncomeOfGreenBean(d),

                            // Tổng đậu đỏ thu được từ việc Student đặt Order
                            Red = orderTransactionRepository.IncomeOfRedBean(d),

                            // Ngày diễn ra
                            Date = d,
                        });
                    }
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy quản trị viên");
            case Role.Brand:
                Brand brand = brandRepository.GetById(id);
                if (brand != null)
                {
                    for (DateOnly d = date.AddDays(-6); d <= date; d = d.AddDays(1))
                    {
                        result.Add(new()
                        {
                            // Tổng đậu xanh thu được từ Request (Admin tạo) và Campaign hoàn trả đậu khi kết thúc
                            Green = requestTransactionRepository.IncomeOfGreenBean(id, d) + campaignTransactionRepository.IncomeOfGreenBean(id, d),

                            // Tổng đậu xanh chi ra cho việc tạo Campaign của Brand
                            Red = campaignTransactionRepository.OutcomeOfGreenBean(id, d),

                            // Ngày diễn ra
                            Date = d,
                        });
                    }
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy thương hiệu");
            case Role.Staff:
                Staff staff = staffRepository.GetById(id);
                if (staff != null)
                {
                    for (DateOnly d = date.AddDays(-6); d <= date; d = d.AddDays(1))
                    {
                        result.Add(new()
                        {
                            // Tổng đậu đỏ thu được từ việc Student đặt hàng cho Station
                            Green = orderTransactionRepository.IncomeOfRedBean(staff.StationId, d),

                            // Tổng đậu đỏ chi ra cho việc hoàn trả đậu cho Student sau khi Staff thuộc Station hủy đơn
                            Red = orderTransactionRepository.OutcomeOfRedBean(staff.StationId, d),

                            // Ngày diễn ra
                            Date = d,
                        });
                    }
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy nhân viên");
            case Role.Store:
                Store store = storeRepository.GetById(id);
                if (store != null)
                {
                    for (DateOnly d = date.AddDays(-6); d <= date; d = d.AddDays(1))
                    {
                        result.Add(new()
                        {
                            // Tổng đậu xanh chi cho việc tặng Bonus bởi Store cho Student
                            Green = bonusTransactionRepository.OutcomeOfGreenBean(store.Id, d),

                            // Tổng đậu xanh chi cho việc Student sử dụng Voucher Item tại Store
                            Red = activityTransactionRepository.OutcomeOfGreenBean(store.Id, d),

                            // Ngày diễn ra
                            Date = d,
                        });
                    }
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy cửa hàng");
            default:
                throw new InvalidParameterException("Xác thực không hợp lệ");
        }
    }

    public List<RankingModel> GetRankingChart(string id, Type type, Role role)
    {
        List<RankingModel> result = new();
        switch (role)
        {
            case Role.Admin:
                Admin admin = adminRepository.GetById(id);
                if (admin != null)
                {
                    if (type.Equals(typeof(Brand)))
                    {
                        // Danh sách 10 thương hiệu tiêu nhiều đậu xanh nhất
                        var source = brandRepository.GetRanking(10);
                        var num = source.GroupBy(r => r.TotalSpending).Select((r, index) => (r , index + 1));
                        result.AddRange(source.Select((r, index) => new RankingModel()
                        {
                            Rank = num.First(n => n.r.Key.Equals(r.TotalSpending)).Item2,
                            Name = r.BrandName,
                            Image = r.Account.Avatar,
                            Value = r.TotalSpending
                        }));
                    }
                    else if (type.Equals(typeof(Student)))
                    {
                        // Danh sách 10 sinh viên tiêu nhiều đậu xanh nhất
                        var source = studentRepository.GetRanking(10);
                        var num = source.GroupBy(r => r.TotalSpending).Select((r, index) => (r, index + 1));
                        result.AddRange(source.Select((r, index) => new RankingModel()
                        {
                            Rank = num.First(n => n.r.Key.Equals(r.TotalSpending)).Item2,
                            Name = r.FullName,
                            Image = r.Account.Avatar,
                            Value = r.TotalSpending
                        }));
                    }
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy quản trị viên");
            case Role.Brand:
                Brand brand = brandRepository.GetById(id);
                if (brand != null)
                {
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy thương hiệu");
            case Role.Staff:
                Staff staff = staffRepository.GetById(id);
                if (staff != null)
                {
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy nhân viên");
            case Role.Store:
                Store store = storeRepository.GetById(id);
                if (store != null)
                {
                    return result;
                }
                throw new InvalidParameterException("Không tìm thấy cửa hàng");
            default:
                throw new InvalidParameterException("Xác thực không hợp lệ");
        }
    }

    public TitleAdminModel GetTitleAdmin(string adminId)
    {
        Admin entity = adminRepository.GetById(adminId);
        if (entity != null)
        {
            return new()
            {
                NumberOfBrands = brandRepository.CountBrand(),
                NumberOfCampagins = campaignRepository.CountCampaign(),
                NumberOfProducts = productRepository.CountProduct(),
                NumberOfStudents = studentRepository.CountStudent()
            };
        }
        throw new InvalidParameterException("Không tìm thấy quản trị viên");
    }

    public TitleBrandModel GetTitleBrand(string brandId)
    {
        Brand entity = brandRepository.GetById(brandId);
        if (entity != null)
        {
            return mapper.Map<TitleBrandModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy thương hiệu");
    }

    public TitleStaffModel GetTitleStaff(string staffId)
    {
        Staff entity = staffRepository.GetById(staffId);
        if (entity != null)
        {
            return mapper.Map<TitleStaffModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy nhân viên");
    }

    public TitleStoreModel GetTitleStore(string storeId)
    {
        Store entity = storeRepository.GetById(storeId);
        if (entity != null)
        {
            return mapper.Map<TitleStoreModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }
}
