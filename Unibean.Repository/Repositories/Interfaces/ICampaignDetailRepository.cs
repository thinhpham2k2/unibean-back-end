using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignDetailRepository
{
    CampaignDetail Add(CampaignDetail creation);

    void Delete(string id);

    PagedResultModel<CampaignDetail> GetAll
        (List<string> campaignIds, List<string> typeIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit);

    List<string> GetAllVoucherItemByCampaignDetail(string id);

    CampaignDetail GetById(string id);

    CampaignDetail Update(CampaignDetail update);
}
