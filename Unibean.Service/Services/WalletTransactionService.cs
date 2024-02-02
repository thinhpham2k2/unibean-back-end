using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class WalletTransactionService : IWalletTransactionService
{
    private readonly Mapper mapper;

    private readonly IWalletTransactionRepository walletTransactionRepository;

    public WalletTransactionService(IWalletTransactionRepository walletTransactionRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<CampaignTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src
                => src.Amount > 0 ? 
                "Chiến dịch " + src.Campaign.CampaignName + " kết thúc" : "Tạo chiến dịch " + src.Campaign.CampaignName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.CampaignId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Amount > 0 ? "Hoàn trả đậu" : "Tạo chiến dịch"))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.walletTransactionRepository = walletTransactionRepository;
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> campaignIds, List<string> walletTypeIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(walletTransactionRepository.GetAll
            (walletIds, campaignIds, walletTypeIds, search));
    }
}
