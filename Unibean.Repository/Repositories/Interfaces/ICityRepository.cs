using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICityRepository
{
    City Add(City creation);

    void Delete(string id);

    PagedResultModel<City> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    City GetById(string id);

    City Update(City update);
}
