using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.CampaignActivities;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignActivityService
{
    PagedResultModel<CampaignActivityModel> GetAll
    (List<string> campaignIds, List<CampaignState> stateIds, 
    string propertySort, bool isAsc, string search, int page, int limit);

    CampaignActivityModel GetById(string id);
}
