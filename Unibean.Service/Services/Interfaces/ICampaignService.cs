using Unibean.Repository.Paging;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignService
{
    PagedResultModel<CampaignModel> GetAll
        (List<string> brandIds, List<string> typeIds, string propertySort,
        bool isAsc, string search, int page, int limit);
}
