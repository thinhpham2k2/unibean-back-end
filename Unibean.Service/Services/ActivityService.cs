using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Activity;
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
            .ForMember(t => t.TypeId, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
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

    public List<StoreTransactionModel> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search)
    {
        return mapper.Map<List<StoreTransactionModel>>(activityRepository.GetList
            (storeIds, studentIds, voucherIds, search));
    }
}
