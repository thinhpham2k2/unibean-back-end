using AutoMapper;
using Enable.EnumDisplayName;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly IMapper mapper;

    private readonly ITransactionRepository transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<object, TransactionModel>()
            .ForMember(t => t.Id, opt => opt.MapFrom(
                src => src.GetType().GetProperty("Id").GetValue(src)))
            .ForMember(t => t.Name, opt => opt.MapFrom((src, dest) =>
            {
                return src switch
                {
                    ActivityTransaction
                    model => model.Activity.VoucherItem.Voucher.VoucherName,

                    BonusTransaction
                    model => model.Bonus.Brand.BrandName + " - " + model.Description,

                    CampaignTransaction
                    model => model.Amount > 0 ?
                    "Chiến dịch " + model.Campaign.CampaignName + " kết thúc" : "Tạo chiến dịch " + model.Campaign.CampaignName,

                    ChallengeTransaction
                    model => model.Challenge.Challenge.ChallengeName,

                    OrderTransaction
                    model => "Tạo đơn hàng",

                    RequestTransaction
                    model => "Nạp đậu",

                    _ => null,
                };
            }))
            .ForMember(t => t.RequestId, opt => opt.MapFrom((src, dest) =>
            {
                return src switch
                {
                    ActivityTransaction
                    model => model.ActivityId,

                    BonusTransaction
                    model => model.BonusId,

                    CampaignTransaction
                    model => model.CampaignId,

                    ChallengeTransaction
                    model => model.ChallengeId,

                    OrderTransaction
                    model => model.OrderId,

                    RequestTransaction
                    model => model.RequestId,

                    _ => null,
                };
            }))
            .ForMember(t => t.WalletId, opt => opt.MapFrom(
                src => src.GetType().GetProperty("WalletId").GetValue(src)))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(
                src => (int)src.GetType().GetProperty("Wallet").GetValue(src)
                .GetType().GetProperty("Type").GetValue(src
                .GetType().GetProperty("Wallet").GetValue(src))))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(
                src => src.GetType().GetProperty("Wallet").GetValue(src)
                .GetType().GetProperty("Type").GetValue(src
                .GetType().GetProperty("Wallet").GetValue(src))))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom((src, dest) =>
            {
                return src switch
                {
                    ActivityTransaction
                    model => model.Wallet.Type.GetDisplayName(),

                    BonusTransaction
                    model => model.Wallet.Type.GetDisplayName(),

                    CampaignTransaction
                    model => model.Wallet.Type.GetDisplayName(),

                    ChallengeTransaction
                    model => model.Wallet.Type.GetDisplayName(),

                    OrderTransaction
                    model => model.Wallet.Type.GetDisplayName(),

                    RequestTransaction
                    model => model.Wallet.Type.GetDisplayName(),

                    _ => null,
                };
            }))
            .ForMember(t => t.TypeName, opt => opt.MapFrom((src, dest) =>
            {
                return src switch
                {
                    ActivityTransaction
                    model => model.Activity.Type.GetEnumDescription(),

                    BonusTransaction
                    model => "Thưởng đậu",

                    CampaignTransaction
                    model => model.Amount > 0 ? "Hoàn trả đậu" : "Tạo chiến dịch",

                    ChallengeTransaction
                    model => "Thử thách",

                    OrderTransaction
                    model => "Đổi quà",

                    RequestTransaction
                    model => "Nạp đậu",

                    _ => null,
                };
            }))
            .ForMember(t => t.Amount, opt => opt.MapFrom(
                src => src.GetType().GetProperty("Amount").GetValue(src)))
            .ForMember(t => t.Rate, opt => opt.MapFrom(
                src => src.GetType().GetProperty("Rate").GetValue(src)))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom((src, dest) =>
            {
                return src switch
                {
                    ActivityTransaction
                    model => model.Activity.DateCreated,

                    BonusTransaction
                    model => model.Bonus.DateCreated,

                    CampaignTransaction
                    model => model.DateCreated,

                    ChallengeTransaction
                    model => model.DateCreated,

                    OrderTransaction
                    model => model.Order.DateCreated,

                    RequestTransaction
                    model => model.Request.DateCreated,

                    _ => null,
                };
            }))
            .ForMember(t => t.Description, opt => opt.MapFrom(
                src => src.GetType().GetProperty("Description").GetValue(src)))
            .ForMember(t => t.State, opt => opt.MapFrom(
                src => src.GetType().GetProperty("State").GetValue(src)))
            .ForMember(t => t.Status, opt => opt.MapFrom(
                src => src.GetType().GetProperty("Status").GetValue(src)))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Object>, PagedResultModel<TransactionModel>>()
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.transactionRepository = transactionRepository;
    }

    public PagedResultModel<TransactionModel> GetAll
        (List<string> walletIds, List<TransactionType> typeIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit, Role role)
    {
        var query = mapper.Map<List<TransactionModel>>(transactionRepository
            .GetAll(walletIds, typeIds, search, role)).AsQueryable()
            .Where(t => state == null || state.Equals(t.State))
            .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

        var result = query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToList();

        return new()
        {
            CurrentPage = page,
            PageSize = limit,
            PageCount = (int)Math.Ceiling((double)query.Count() / limit),
            Result = result,
            RowCount = result.Count,
            TotalCount = query.Count()
        };
    }
}
