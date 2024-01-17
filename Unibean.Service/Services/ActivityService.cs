using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class ActivityService : IActivityService
{
    private readonly Mapper mapper;

    private readonly IActivityRepository activityRepository;

    public ActivityService(IActivityRepository activityRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<Activity, StoreTransactionModel>()
            .ForMember(t => t.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(t => t.Activity, opt => opt.MapFrom(src => " đã sử dụng " + src.VoucherItem.Voucher.VoucherName + " tại "))
            .ForMember(t => t.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(t => t.Amount, opt => opt.MapFrom(src => src.ActivityTransactions.FirstOrDefault().Amount))
            .ForMember(t => t.Rate, opt => opt.MapFrom(src => src.ActivityTransactions.FirstOrDefault().Rate))
            .ForMember(t => t.WalletId, opt => opt.MapFrom(src => src.ActivityTransactions.FirstOrDefault().WalletId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.ActivityTransactions.FirstOrDefault().Wallet.Type))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.ActivityTransactions.FirstOrDefault().Wallet.Type.Image))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.activityRepository = activityRepository;
    }

    public List<StoreTransactionModel> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search)
    {
        return mapper.Map<List<StoreTransactionModel>>(activityRepository.GetList
            (storeIds, studentIds, voucherIds, search));
    }
}
