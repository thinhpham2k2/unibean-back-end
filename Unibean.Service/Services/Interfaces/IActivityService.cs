using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IActivityService
{
    List<StoreTransactionModel> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search);
}
