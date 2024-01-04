using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class ActivityTransactionService : IActivityTransactionService
{
    private readonly Mapper mapper;

    private readonly IActivityTransactionRepository activityTransactionRepo;

    public ActivityTransactionService(
        IActivityTransactionRepository activityTransactionRepo)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<ActivityTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Activity.VoucherItem.Voucher.VoucherName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.ActivityId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Activity.Type.TypeName))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Activity.DateCreated))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.activityTransactionRepo = activityTransactionRepo;
    }

    public List<TransactionModel> GetAll(List<string> walletIds, List<string> activityIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(activityTransactionRepo.GetAll
            (walletIds, activityIds, search));
    }
}
