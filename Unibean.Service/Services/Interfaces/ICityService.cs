using Unibean.Repository.Paging;
using Unibean.Service.Models.Cities;

namespace Unibean.Service.Services.Interfaces;

public interface ICityService
{
    Task<CityModel> Add(CreateCityModel creation);

    void Delete(string id);

    PagedResultModel<CityModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    CityModel GetById(string id);

    Task<CityModel> Update(string id, UpdateCityModel update);
}
