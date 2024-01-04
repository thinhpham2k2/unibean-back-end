using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IActivityTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> activityIds, string search);
}
