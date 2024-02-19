using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class BonusTransactionService : IBonusTransactionService
{
    private readonly Mapper mapper;

    private readonly IBonusTransactionRepository bonusTransactionRepository;

    public BonusTransactionService(IBonusTransactionRepository bonusTransactionRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<BonusTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(
                src => src.Bonus.Brand.BrandName + " - " + src.Description))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.BonusId))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(src => (int)src.Wallet.Type))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom(
                src => src.Wallet.Type.GetDisplayName()))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Thưởng đậu"))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Bonus.DateCreated))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.bonusTransactionRepository = bonusTransactionRepository;
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> bonusIds, List<WalletType> walletTypeIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(bonusTransactionRepository.GetAll
            (walletIds, bonusIds, walletTypeIds, search));
    }
}
