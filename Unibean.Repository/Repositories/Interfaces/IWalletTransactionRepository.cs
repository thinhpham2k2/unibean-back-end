using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IWalletTransactionRepository
{
    CampaignTransaction Add(CampaignTransaction creation);

    List<CampaignTransaction> GetAll
        (List<string> walletIds, List<string> campaignIds, List<string> walletTypeIds, string search);

    CampaignTransaction GetById(string id);
}
