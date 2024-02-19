using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignActivityRepository
{
    CampaignActivity Add(CampaignActivity creation);

    void Delete(string id);

    PagedResultModel<CampaignActivity> GetAll
        (List<string> campaignIds, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit);

    CampaignActivity GetById(string id);

    CampaignActivity Update(CampaignActivity update);
}
