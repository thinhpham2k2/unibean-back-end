using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IWalletTransactionRepository
{
    WalletTransaction Add(WalletTransaction creation);

    List<WalletTransaction> GetAll
        (List<string> walletIds, List<string> campaignIds, string search);

    WalletTransaction GetById(string id);
}
