using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IChallengeTransactionRepository
{
    ChallengeTransaction Add(ChallengeTransaction creation);

    List<ChallengeTransaction> GetAll
        (List<string> walletIds, List<string> challengeIds, string search);

    ChallengeTransaction GetById(string id);
}
