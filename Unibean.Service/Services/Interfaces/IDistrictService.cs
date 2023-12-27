using Unibean.Repository.Paging;
using Unibean.Service.Models.Districts;

namespace Unibean.Service.Services.Interfaces;

public interface IDistrictService
{
    Task<DistrictModel> Add(CreateDistrictModel creation);

    void Delete(string id);

    PagedResultModel<DistrictModel> GetAll
        (List<string> cityIds, string propertySort, bool isAsc, string search, int page, int limit);

    DistrictModel GetById(string id);

    Task<DistrictModel> Update(string id, UpdateDistrictModel update);
}
