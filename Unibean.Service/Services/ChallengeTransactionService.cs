using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class ChallengeTransactionService : IChallengeTransactionService
{
    private readonly Mapper mapper;

    private readonly IChallengeTransactionRepository challengeTransRepo;

    public ChallengeTransactionService(
        IChallengeTransactionRepository challengeTransRepo)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<ChallengeTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Challenge.Challenge.ChallengeName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.ChallengeId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Thử thách"))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.challengeTransRepo = challengeTransRepo;
    }

    public List<TransactionModel> GetAll(List<string> walletIds, List<string> challengeIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(challengeTransRepo.GetAll
            (walletIds, challengeIds, search));
    }
}
