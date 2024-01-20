using Unibean.Repository.Entities;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IRequestTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> requestIds, List<string> walletTypeIds, string search);
}
