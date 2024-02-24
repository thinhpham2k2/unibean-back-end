using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.ChallengeTransactions;
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
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(src => (int)src.Wallet.Type))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom(src => src.Wallet.Type.GetDisplayName()))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Thử thách"))
            .ReverseMap();
            cfg.CreateMap<ChallengeTransaction, ChallengeTransactionModel>()
            .ForMember(t => t.ChallengeName, opt => opt.MapFrom(src => src.Challenge.Challenge.ChallengeName))
            .ReverseMap();
            cfg.CreateMap<ChallengeTransaction, CreateChallengeTransactionModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.challengeTransRepo = challengeTransRepo;
    }

    public ChallengeTransactionModel Add(CreateChallengeTransactionModel creation)
    {
        return mapper.Map<ChallengeTransactionModel>
            (challengeTransRepo.Add(mapper.Map<ChallengeTransaction>(creation)));
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> challengeIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(challengeTransRepo.GetAll
            (walletIds, challengeIds, search));
    }
}
