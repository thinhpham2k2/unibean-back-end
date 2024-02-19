using Unibean.Repository.Paging;
using Unibean.Service.Models.Universities;

namespace Unibean.Service.Services.Interfaces;

public interface IUniversityService
{
    Task<UniversityExtraModel> Add(CreateUniversityModel creation);

    void Delete(string id);

    PagedResultModel<UniversityModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    UniversityExtraModel GetById(string id);

    Task<UniversityExtraModel> Update(string id, UpdateUniversityModel update);
}
