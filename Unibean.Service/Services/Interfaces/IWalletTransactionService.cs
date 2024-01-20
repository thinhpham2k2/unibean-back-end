using Unibean.Repository.Entities;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IWalletTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> campaignIds, List<string> walletTypeIds, string search);
}
