using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Charts;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

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

    private readonly IStudentRepository studentRepository;

    public ChartService(
        IAdminRepository adminRepository,
        IBrandRepository brandRepository,
        ICampaignRepository campaignRepository,
        IProductRepository productRepository,
        IStaffRepository staffRepository,
        IStoreRepository storeRepository,
        IStudentRepository studentRepository)
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
        this.studentRepository = studentRepository;
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
