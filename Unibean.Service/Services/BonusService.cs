using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class BonusService : IBonusService
{
    private readonly Mapper mapper;

    private readonly IBonusRepository bonusRepository;

    public BonusService(IBonusRepository bonusRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<Bonus, StoreTransactionModel>()
            .ForMember(t => t.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(t => t.Activity, opt => opt.MapFrom(src => " nhận được bonus tại "))
            .ForMember(t => t.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(t => t.Amount, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().Amount))
            .ForMember(t => t.Rate, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().Rate))
            .ForMember(t => t.WalletId, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().WalletId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.BonusTransactions.FirstOrDefault().Wallet.Type.Image))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.bonusRepository = bonusRepository;
    }

    public List<StoreTransactionModel> GetList
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, string search)
    {
        return mapper.Map<List<StoreTransactionModel>>(bonusRepository.GetList
            (brandIds, storeIds, studentIds, search));
    }
}
