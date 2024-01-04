using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class PaymentTransactionService : IPaymentTransactionService
{
    private readonly Mapper mapper;

    private readonly IPaymentTransactionRepository paymentTransactionRepo;

    public PaymentTransactionService(
        IPaymentTransactionRepository paymentTransactionRepo)
    {
        var config = new MapperConfiguration(cfg 
            =>
        {
            cfg.CreateMap<PaymentTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => "Nạp đậu (" + ((decimal)src.Amount).ToString("N") + " đậu)"))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.PaymentId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Thanh toán"))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Payment.DateCreated))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.paymentTransactionRepo = paymentTransactionRepo;
    }

    public List<TransactionModel> GetAll(List<string> walletIds, List<string> paymentIds, string search)
    { 
        return mapper.Map<List<TransactionModel>>(paymentTransactionRepo.GetAll
            (walletIds, paymentIds, search));
    }
}
