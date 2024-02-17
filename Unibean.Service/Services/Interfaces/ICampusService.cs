using Unibean.Repository.Paging;
using Unibean.Service.Models.Campuses;

namespace Unibean.Service.Services.Interfaces;

public interface ICampusService
{
    Task<CampusExtraModel> Add(CreateCampusModel creation);

    void Delete(string id);

    PagedResultModel<CampusModel> GetAll
        (List<string> universityIds, List<string> areaIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<CampusModel> GetAllByCampaign
        (List<string> campaignIds, List<string> universityIds, List<string> areaIds,
         bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    CampusExtraModel GetById(string id);

    Task<CampusExtraModel> Update(string id, UpdateCampusModel update);
}
