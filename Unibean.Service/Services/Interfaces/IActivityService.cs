using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Activity;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IActivityService
{
    ActivityModel Add(CreateActivityModel creation);

    List<StoreTransactionModel> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search);
}
