using Unibean.Repository.Entities;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IBonusTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> bonusIds, List<WalletType> walletTypeIds, string search);
}
