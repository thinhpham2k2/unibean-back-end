using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Transactions;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Services.Interfaces;

public interface IActivityService
{
    ActivityModel Add(CreateActivityModel creation);

    PagedResultModel<ActivityModel> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, List<string> campaignIds,
        List<string> campaignDetailIds, List<string> voucherIds, List<string> voucherItemIds, List<Type> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    ActivityExtraModel GetById(string id);

    List<StoreTransactionModel> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search);
}
