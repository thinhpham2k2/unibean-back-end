using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignTypeRepository
{
    CampaignType Add(CampaignType creation);

    void Delete(string id);

    PagedResultModel<CampaignType> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    CampaignType GetById(string id);

    CampaignType Update(CampaignType update);
}
