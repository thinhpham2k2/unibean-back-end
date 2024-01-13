using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IActivityTransactionRepository
{
    ActivityTransaction Add(ActivityTransaction creation);

    List<ActivityTransaction> GetAll
        (List<string> walletIds, List<string> activityIds, string search);

    ActivityTransaction GetById(string id);
}
