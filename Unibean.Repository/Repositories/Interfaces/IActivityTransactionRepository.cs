using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IActivityTransactionRepository
{
    List<ActivityTransaction> GetAll
        (List<string> walletIds, List<string> activityIds, string search);
}
