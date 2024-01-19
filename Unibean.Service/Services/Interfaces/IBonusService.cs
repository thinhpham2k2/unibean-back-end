using Unibean.Repository.Paging;
using Unibean.Service.Models.Bonuses;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IBonusService
{
    BonusExtraModel Add(string id, CreateBonusModel creation);

    PagedResultModel<BonusModel> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds,
        string propertySort, bool isAsc, string search, int page, int limit);

    BonusExtraModel GetById(string id);

    List<StoreTransactionModel> GetList
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, string search);
}
