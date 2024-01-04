using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IOrderTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> orderIds, string search);
}
