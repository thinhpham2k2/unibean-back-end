using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignRepository
{
    Campaign Add(Campaign creation);

    List<string> AllToClosed(string id);

    long CountCampaign();

    void Delete(string id);

    bool ExpiredToClosed(string id);

    PagedResultModel<Campaign> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds,
        List<string> majorIds, List<string> campusIds, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit);

    List<Campaign> GetAllEnded(List<CampaignState> stateIds);

    List<Campaign> GetAllExpired(List<CampaignState> stateIds, DateOnly date);

    Campaign GetById(string id);

    List<Campaign> GetRanking(string brandId, int limit);

    Campaign Update(Campaign update);
}
