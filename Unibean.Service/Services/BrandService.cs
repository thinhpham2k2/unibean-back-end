using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Types;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Models.Wallets;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class BrandService : IBrandService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "brands";

    private readonly IBrandRepository brandRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IWalletService walletService;

    private readonly IWalletTypeService walletTypeService;

    public BrandService(IBrandRepository brandRepository, 
        IFireBaseService fireBaseService,
        IWalletService walletService,
        IWalletTypeService walletTypeService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            // Map Brand Model
            cfg.CreateMap<Brand, BrandModel>()
            .ForMember(p => p.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(p => p.Logo, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(p => p.LogoFileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(p => p.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Brand>, PagedResultModel<BrandModel>>()
            .ReverseMap();
            // Map Brand Extra Model
            cfg.CreateMap<Brand, BrandExtraModel>()
            .ForMember(p => p.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(p => p.Logo, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(p => p.LogoFileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(p => p.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(p => p.NumberOfFollowers, opt => opt.MapFrom(src => src.Wishlists.Count))
            .ReverseMap();
            cfg.CreateMap<Wallet, WalletModel>()
            .ForMember(w => w.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            cfg.CreateMap<Campaign, CampaignModel>()
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            cfg.CreateMap<Store, StoreModel>()
            .ForMember(s => s.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ReverseMap();
            cfg.CreateMap<Voucher, VoucherModel>()
            .ForMember(s => s.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            // Map Create Brand Google Model
            cfg.CreateMap<Brand, CreateBrandGoogleModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            // Map Create Brand Model
            cfg.CreateMap<Brand, CreateBrandModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            ;
        });
        mapper = new Mapper(config);
        this.brandRepository = brandRepository;
        this.fireBaseService = fireBaseService;
        this.walletService = walletService;
        this.walletTypeService = walletTypeService;
    }

    public Task<BrandModel> Add(CreateTypeModel creation)
    {
        throw new NotImplementedException();
    }

    public BrandModel AddGoogle(CreateBrandGoogleModel creation)
    {
        Brand entity = brandRepository.Add(mapper.Map<Brand>(creation));

        // Create wallet
        if (entity != null)
        {
            walletService.Add(new CreateWalletModel
            {
                BrandId = entity.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Green bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
            walletService.Add(new CreateWalletModel
            {
                BrandId = entity.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Red bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
        }
        return mapper.Map<BrandModel>(entity);
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public PagedResultModel<BrandModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        throw new NotImplementedException();
    }

    public BrandExtraModel GetById(string id)
    {
        Brand entity = brandRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<BrandExtraModel>(entity);
        }
        throw new InvalidParameterException("Not found brand");
    }

    public Task<BrandModel> Update(string id, UpdateTypeModel update)
    {
        throw new NotImplementedException();
    }
}
