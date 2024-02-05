using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Bonuses;
using Unibean.Service.Models.BonusTransactions;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class BonusService : IBonusService
{
    private readonly Mapper mapper;

    private readonly IBonusRepository bonusRepository;

    private readonly IStoreRepository storeRepository;

    public BonusService(IBonusRepository bonusRepository, 
        IStoreRepository storeRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<Bonus, BonusModel>()
            .ForMember(r => r.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(r => r.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(r => r.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Bonus>, PagedResultModel<BonusModel>>()
            .ReverseMap();
            cfg.CreateMap<Bonus, BonusExtraModel>()
            .ForMember(r => r.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(r => r.BrandAcronym, opt => opt.MapFrom(src => src.Brand.Acronym))
            .ForMember(r => r.BrandLogo, opt => opt.MapFrom(src => src.Brand.Account.Avatar))
            .ForMember(r => r.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(r => r.StoreImage, opt => opt.MapFrom(src => src.Store.Account.Avatar))
            .ForMember(r => r.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(r => r.StudentAvatar, opt => opt.MapFrom(src => src.Student.Account.Avatar))
            .ReverseMap();
            cfg.CreateMap<BonusTransaction, BonusTransactionModel>()
            .ForMember(r => r.WalletTypeId, opt => opt.MapFrom(src => (int)src.Wallet.Type))
            .ForMember(r => r.WalletType, opt => opt.MapFrom(src => src.Wallet.Type))
            .ForMember(r => r.WalletTypeName, opt => opt.MapFrom(
                src => src.Wallet.Type.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<Bonus, CreateBonusModel>()
            .ReverseMap()
            .ForMember(r => r.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(r => r.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(r => r.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(r => r.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Bonus, StoreTransactionModel>()
            .ForMember(t => t.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(t => t.Activity, opt => opt.MapFrom(src => " nhận được bonus tại "))
            .ForMember(t => t.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(t => t.Amount, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().Amount))
            .ForMember(t => t.Rate, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().Rate))
            .ForMember(t => t.WalletId, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().WalletId))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(
                src => (int)src.BonusTransactions.FirstOrDefault().Wallet.Type))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(
                src => src.BonusTransactions.FirstOrDefault().Wallet.Type))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom(
                src => src.BonusTransactions.FirstOrDefault().Wallet.Type.GetDisplayName()))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.bonusRepository = bonusRepository;
        this.storeRepository = storeRepository;
    }

    public BonusExtraModel Add(string id, CreateBonusModel creation)
    {
        Store store = storeRepository.GetById(id);
        if (store != null)
        {
            if(store.Brand.Wallets.FirstOrDefault().Balance >= creation.Amount)
            {
                Bonus bonus = mapper.Map<Bonus>(creation);
                bonus.StoreId = id;
                bonus.BrandId = store.BrandId;
                return mapper.Map<BonusExtraModel>(bonusRepository.Add(bonus));
            }
            throw new InvalidParameterException("Số dư ví đậu xanh của thương hiệu là không đủ");
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }

    public PagedResultModel<BonusModel> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<BonusModel>>(bonusRepository
            .GetAll(brandIds, storeIds, studentIds, state, propertySort, isAsc, search, page, limit));
    }

    public BonusExtraModel GetById(string id)
    {
        Bonus entity = bonusRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<BonusExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy tiền thưởng");
    }

    public List<StoreTransactionModel> GetList
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, string search)
    {
        return mapper.Map<List<StoreTransactionModel>>(bonusRepository.GetList
            (brandIds, storeIds, studentIds, search));
    }
}
