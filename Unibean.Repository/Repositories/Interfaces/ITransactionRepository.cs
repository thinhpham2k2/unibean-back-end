using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public enum TransactionType
{
    ActivityTransaction = 1,
    OrderTransaction = 2,
    ChallengeTransaction = 3,
    BonusTransaction = 4
}

public interface ITransactionRepository
{
    List<object> GetAll
        (List<string> walletIds, List<TransactionType> typeIds, string search, Role role);
}
