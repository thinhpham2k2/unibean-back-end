using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IBonusTransactionRepository
{
    List<BonusTransaction> GetAll
        (List<string> walletIds, List<string> bonusIds, List<WalletType> walletTypeIds, string search);
}
