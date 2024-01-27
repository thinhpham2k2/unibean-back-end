using Unibean.Repository.Paging;
using Unibean.Service.Models.CampaignTypes;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignTypeService
{
    Task<CampaignTypeModel> Add(CreateCampaignTypeModel creation);

    void Delete(string id);

    PagedResultModel<CampaignTypeModel> GetAll
        (bool? state, string propertySort, bool isAsc, 
        string search, int page, int limit);

    CampaignTypeModel GetById(string id);

    Task<CampaignTypeModel> Update(string id, UpdateCampaignTypeModel update);
}
