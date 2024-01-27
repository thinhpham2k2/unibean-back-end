using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampusRepository
{
    Campus Add(Campus creation);

    void Delete(string id);

    PagedResultModel<Campus> GetAll
        (List<string> universityIds, List<string> areaIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<Campus> GetAllByCampaign
        (List<string> campaignIds, List<string> universityIds, List<string> areaIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    Campus GetById(string id);

    Campus Update(Campus update);
}
