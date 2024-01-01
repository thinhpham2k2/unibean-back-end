using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IChallengeTransactionRepository
{
    ChallengeTransaction Add(ChallengeTransaction creation);

    PagedResultModel<ChallengeTransaction> GetAll
        (List<string> walletIds, List<string> challengeIds, string propertySort, bool isAsc, string search, int page, int limit);

    ChallengeTransaction GetById(string id);
}
