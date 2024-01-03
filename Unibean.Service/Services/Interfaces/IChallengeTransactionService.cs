using Unibean.Repository.Entities;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface IChallengeTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> challengeIds, string search);
}
