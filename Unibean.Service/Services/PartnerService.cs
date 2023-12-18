using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Partners;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Wallets;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public class PartnerService : IPartnerService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "partners";

    private readonly IPartnerRepository partnerRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IWalletService walletService;

    private readonly IWalletTypeService walletTypeService;

    public PartnerService(IPartnerRepository partnerRepository, 
        IFireBaseService fireBaseService,
        IWalletService walletService,
        IWalletTypeService walletTypeService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Partner, PartnerModel>().ReverseMap();
            cfg.CreateMap<Wallet, WalletModel>()
            .ForMember(w => w.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            cfg.CreateMap<Campaign, CampaignModel>()
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            cfg.CreateMap<Store, StoreModel>()
            .ForMember(s => s.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ReverseMap();
            cfg.CreateMap<Partner, PartnerModel>().ReverseMap();
            cfg.CreateMap<Partner, PartnerExtraModel>()
            .ForMember(p => p.NumberOfFollowers, opt => opt.MapFrom(src => src.Wishlists.Count))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Partner>, PagedResultModel<PartnerModel>>()
            .ReverseMap();
            cfg.CreateMap<Partner, CreatePartnerModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(p => p.Logo, opt => opt.Ignore())
            .ForMember(p => p.CoverPhoto, opt => opt.Ignore())
            .ForMember(p => p.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.partnerRepository = partnerRepository;
        this.fireBaseService = fireBaseService;
        this.walletService = walletService;
        this.walletTypeService = walletTypeService;
    }

    public async Task<PartnerExtraModel> Add(CreatePartnerModel creation)
    {
        Partner entity = mapper.Map<Partner>(creation);
        
        // Upload logo image
        if (creation.Logo != null && creation.Logo.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Logo, FOLDER_NAME);
            entity.Logo = f.URL;
            entity.LogoFileName = f.FileName;
        }

        // Upload cover photo
        if (creation.CoverPhoto != null && creation.CoverPhoto.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.CoverPhoto, FOLDER_NAME);
            entity.CoverPhoto = f.URL;
            entity.CoverFileName = f.FileName;
        }

        entity = partnerRepository.Add(entity);
        // Create wallet
        if(entity != null)
        {
            walletService.Add(new CreateWalletModel
            {
                PartnerId = entity.Id,
                TypeId = walletTypeService.GetFirst().Id,
                Balance = 0,
                Description = string.Empty,
                State = true
            });
            walletService.Add(new CreateWalletModel
            {
                PartnerId = entity.Id,
                TypeId = walletTypeService.GetSecond().Id,
                Balance = 0,
                Description = string.Empty,
                State = true
            });
        }
        return mapper.Map<PartnerExtraModel>(entity);
    }

    public PartnerExtraModel GetById(string id)
    {
        Partner entity = partnerRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<PartnerExtraModel>(entity);
        }
        throw new InvalidParameterException("Not found partner");
    }

    public PartnerModel GetByUserNameAndPassword(string userName, string password)
    {
        return mapper.Map<PartnerModel>(partnerRepository.GetByUserNameAndPassword(userName, password));
    }
}
