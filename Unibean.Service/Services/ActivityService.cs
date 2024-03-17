using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Services;

public class ActivityService : IActivityService
{
    private readonly Mapper mapper;

    private readonly IActivityRepository activityRepository;

    private readonly IVoucherItemRepository voucherItemRepository;

    public ActivityService(
        IActivityRepository activityRepository,
        IVoucherItemRepository voucherItemRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<Activity, StoreTransactionModel>()
            .ForMember(t => t.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(t => t.Activity, opt => opt.MapFrom(
                src => " sử dụng " + src.VoucherItem.Voucher.VoucherName + " tại "))
            .ForMember(t => t.StoreName, opt => opt.MapFrom(
                src => src.Store.StoreName))
            .ForMember(t => t.Amount, opt => opt.MapFrom(
                src => src.ActivityTransactions.FirstOrDefault().Amount))
            .ForMember(t => t.Rate, opt => opt.MapFrom(
                src => src.ActivityTransactions.FirstOrDefault().Rate))
            .ForMember(t => t.WalletId, opt => opt.MapFrom(
                src => src.ActivityTransactions.FirstOrDefault().WalletId))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(
                src => (int)src.ActivityTransactions.FirstOrDefault().Wallet.Type))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(
                src => src.ActivityTransactions.FirstOrDefault().Wallet.Type))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom(
                src => src.ActivityTransactions.FirstOrDefault().Wallet.Type.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<Activity, ActivityModel>()
            .ForMember(t => t.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(t => t.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(t => t.Amount, opt => opt.MapFrom((src, dest) =>
            {
                if (src.Type != null)
                {
                    return src.Type switch
                    {
                        Type.Buy or Type.Refund => src.VoucherItem.CampaignDetail.Price,
                        Type.Use => src.VoucherItem.CampaignDetail.Price * src.VoucherItem.CampaignDetail.Rate,
                        _ => null,
                    };
                }
                return null;
            }))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom((src, dest) =>
            {
                return src.Type switch
                {
                    Type.Buy or Type.Refund => (int)WalletType.Green,
                    Type.Use => (int)WalletType.Red,
                    _ => throw new NotImplementedException(),
                };
            }))
            .ForMember(t => t.WalletType, opt => opt.MapFrom((src, dest) =>
            {
                return src.Type switch
                {
                    Type.Buy or Type.Refund => WalletType.Green,
                    Type.Use => WalletType.Red,
                    _ => throw new NotImplementedException(),
                };
            }))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom((src, dest) =>
            {
                return src.Type switch
                {
                    Type.Buy or Type.Refund => WalletType.Green.GetDisplayName(),
                    Type.Use => WalletType.Red.GetDisplayName(),
                    _ => throw new NotImplementedException(),
                };
            }))
            .ForMember(t => t.TypeId, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(t => t.VoucherName, opt => opt.MapFrom(src => src.VoucherItem.Voucher.VoucherName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Activity>, PagedResultModel<ActivityModel>>()
            .ReverseMap(); cfg.CreateMap<Activity, ActivityExtraModel>()
            .ForMember(t => t.BrandId, opt => opt.MapFrom(src => src.VoucherItem.Voucher.BrandId))
            .ForMember(t => t.BrandName, opt => opt.MapFrom(src => src.VoucherItem.Voucher.Brand.BrandName))
            .ForMember(t => t.BrandImage, opt => opt.MapFrom(src => src.VoucherItem.Voucher.Brand.Account.Avatar))
            .ForMember(t => t.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(t => t.StoreImage, opt => opt.MapFrom(src => src.Store.Account.Avatar))
            .ForMember(t => t.CampaignId, opt => opt.MapFrom(src => src.VoucherItem.CampaignDetail.CampaignId))
            .ForMember(t => t.CampaignName, opt => opt.MapFrom(src => src.VoucherItem.CampaignDetail.Campaign.CampaignName))
            .ForMember(t => t.CampaignImage, opt => opt.MapFrom(src => src.VoucherItem.CampaignDetail.Campaign.Image))
            .ForMember(t => t.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(t => t.StudentImage, opt => opt.MapFrom(src => src.Student.Account.Avatar))
            .ForMember(t => t.Amount, opt => opt.MapFrom((src, dest) =>
            {
                if (src.Type != null)
                {
                    return src.Type switch
                    {
                        Type.Buy or Type.Refund => src.VoucherItem.CampaignDetail.Price,
                        Type.Use => src.VoucherItem.CampaignDetail.Price * src.VoucherItem.CampaignDetail.Rate,
                        _ => null,
                    };
                }
                return null;
            }))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom((src, dest) =>
            {
                return src.Type switch
                {
                    Type.Buy or Type.Refund => (int)WalletType.Green,
                    Type.Use => (int)WalletType.Red,
                    _ => throw new NotImplementedException(),
                };
            }))
            .ForMember(t => t.WalletType, opt => opt.MapFrom((src, dest) =>
            {
                return src.Type switch
                {
                    Type.Buy or Type.Refund => WalletType.Green,
                    Type.Use => WalletType.Red,
                    _ => throw new NotImplementedException(),
                };
            }))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom((src, dest) =>
            {
                return src.Type switch
                {
                    Type.Buy or Type.Refund => WalletType.Green.GetDisplayName(),
                    Type.Use => WalletType.Red.GetDisplayName(),
                    _ => throw new NotImplementedException(),
                };
            }))
            .ForMember(t => t.TypeId, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(t => t.VoucherName, opt => opt.MapFrom(src => src.VoucherItem.Voucher.VoucherName))
            .ForMember(t => t.VoucherCode, opt => opt.MapFrom(src => src.VoucherItem.VoucherCode))
            .ForMember(t => t.VoucherPrice, opt => opt.MapFrom(src => src.VoucherItem.CampaignDetail.Price))
            .ForMember(t => t.VoucherRate, opt => opt.MapFrom(src => src.VoucherItem.CampaignDetail.Rate))
            .ForMember(t => t.VoucherImage, opt => opt.MapFrom(src => src.VoucherItem.Voucher.Image))
            .ReverseMap();
            cfg.CreateMap<Activity, CreateActivityModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.StoreId, opt => opt.MapFrom(src => src.Type.Equals(Type.Use) ? src.StoreId : null))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.activityRepository = activityRepository;
        this.voucherItemRepository = voucherItemRepository;
    }

    public ActivityModel Add(CreateActivityModel creation)
    {
        Activity entity = mapper.Map<Activity>(creation);
        entity.VoucherItem = voucherItemRepository.GetById(creation.VoucherItemId);
        return mapper.Map<ActivityModel>(activityRepository.Add(entity));
    }

    public PagedResultModel<ActivityModel> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds,
        List<string> campaignIds, List<string> campaignDetailIds, List<string> voucherIds,
        List<string> voucherItemIds, List<Type> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<ActivityModel>>(activityRepository.GetAll
            (brandIds, storeIds, studentIds, campaignIds, campaignDetailIds, voucherIds,
            voucherItemIds, typeIds, state, propertySort, isAsc, search, page, limit));
    }

    public ActivityExtraModel GetById(string id)
    {
        Activity entity = activityRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<ActivityExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy hoạt động");
    }

    public List<StoreTransactionModel> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search)
    {
        return mapper.Map<List<StoreTransactionModel>>(activityRepository.GetList
            (storeIds, studentIds, voucherIds, search));
    }
}
