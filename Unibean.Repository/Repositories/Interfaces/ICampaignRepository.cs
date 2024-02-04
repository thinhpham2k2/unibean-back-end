using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignRepository
{
    Campaign Add(Campaign creation);

    void Delete(string id);

    PagedResultModel<Campaign> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds, 
        List<string> majorIds, List<string> campusIds, List<CampaignState> stateIds, 
        string propertySort, bool isAsc, string search, int page, int limit);

    Campaign GetById(string id);

    Campaign Update(Campaign update);
}
