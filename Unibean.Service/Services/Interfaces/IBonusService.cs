using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IBonusService
{
    List<StoreTransactionModel> GetList
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, string search);
}
