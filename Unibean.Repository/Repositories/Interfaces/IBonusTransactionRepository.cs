using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IBonusTransactionRepository
{
    BonusTransaction Add(BonusTransaction creation);

    List<BonusTransaction> GetAll
        (List<string> walletIds, List<string> bonusIds, string search);

    BonusTransaction GetById(string id);
}
