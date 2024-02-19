using Unibean.Repository.Entities;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignTransactionService
{
    List<TransactionModel> GetAll
        (List<string> walletIds, List<string> campaignIds, 
        List<WalletType> walletTypeIds, string search);
}
