using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class RequestTransactionService : IRequestTransactionService
{
    private readonly Mapper mapper;

    private readonly IRequestTransactionRepository requestTransactionRepository;

    public RequestTransactionService(IRequestTransactionRepository requestTransactionRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<RequestTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => "Nạp đậu (" + src.Amount + " đậu)"))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.RequestId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Nạp đậu"))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.requestTransactionRepository = requestTransactionRepository;
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> requestIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(requestTransactionRepository.GetAll
            (walletIds, requestIds, search));
    }
}
