using Unibean.Repository.Paging;
using Unibean.Service.Models.Campuses;

namespace Unibean.Service.Services.Interfaces;

public interface ICampusService
{
    Task<CampusModel> Add(CreateCampusModel creation);

    void Delete(string id);

    PagedResultModel<CampusModel> GetAll
        (List<string> universityIds, List<string> areaIds, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<CampusModel> GetAllByCampaign
        (List<string> campaignIds, List<string> universityIds, List<string> areaIds, 
        string propertySort, bool isAsc, string search, int page, int limit);

    CampusModel GetById(string id);

    Task<CampusModel> Update(string id, UpdateCampusModel update);
}
