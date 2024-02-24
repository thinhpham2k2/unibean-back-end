using Unibean.Repository.Entities;
using Unibean.Service.Models.ChallengeTransactions;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IChallengeTransactionService
{
    ChallengeTransactionModel Add(CreateChallengeTransactionModel creation);

    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> challengeIds, string search);
}
