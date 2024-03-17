using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class CampaignTransactionService : ICampaignTransactionService
{
    private readonly Mapper mapper;

    private readonly ICampaignTransactionRepository campaignTransactionRepository;

    public CampaignTransactionService(ICampaignTransactionRepository campaignTransactionRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<CampaignTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src
                => src.Amount > 0 ?
                "Chiến dịch " + src.Campaign.CampaignName + " kết thúc" : "Tạo chiến dịch " + src.Campaign.CampaignName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.CampaignId))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(src => (int)src.Wallet.Type))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom(src => src.Wallet.Type.GetDisplayName()))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Amount > 0 ? "Hoàn trả đậu" : "Tạo chiến dịch"))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.campaignTransactionRepository = campaignTransactionRepository;
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> campaignIds,
        List<WalletType> walletTypeIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(campaignTransactionRepository.GetAll
            (walletIds, campaignIds, walletTypeIds, search));
    }
}
