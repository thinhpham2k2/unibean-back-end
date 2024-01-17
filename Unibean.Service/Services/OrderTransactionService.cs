using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class OrderTransactionService : IOrderTransactionService
{
    private readonly Mapper mapper;

    private readonly IOrderTransactionRepository orderTransactionRepo;

    public OrderTransactionService(
        IOrderTransactionRepository orderTransactionRepo)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<OrderTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => "Tạo đơn hàng (" + ((decimal)src.Amount).ToString("N") + " đậu)"))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Đổi quà"))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Order.DateCreated))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.orderTransactionRepo = orderTransactionRepo;
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> orderIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(orderTransactionRepo.GetAll
            (walletIds, orderIds, search));
    }
}
