using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignTransactionRepository
{
    CampaignTransaction Add(CampaignTransaction creation);

    List<CampaignTransaction> GetAll
        (List<string> walletIds, List<string> campaignIds, List<WalletType> walletTypeIds, string search);

    CampaignTransaction GetById(string id);
}
