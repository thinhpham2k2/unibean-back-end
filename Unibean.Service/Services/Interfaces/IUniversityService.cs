using Unibean.Repository.Paging;
using Unibean.Service.Models.Universities;

namespace Unibean.Service.Services.Interfaces;

public interface IUniversityService
{
    Task<UniversityModel> Add(CreateUniversityModel creation);

    void Delete(string id);

    PagedResultModel<UniversityModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    UniversityModel GetById(string id);

    Task<UniversityModel> Update(string id, UpdateUniversityModel update);
}
