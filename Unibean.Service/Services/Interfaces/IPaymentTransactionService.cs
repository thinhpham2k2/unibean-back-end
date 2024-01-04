using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IPaymentTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> paymentIds, string search);
}
