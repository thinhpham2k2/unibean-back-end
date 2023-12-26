using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IAreaRepository
{
    Area Add(Area creation);

    void Delete(string id);

    PagedResultModel<Area> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Area GetById(string id);

    Area Update(Area update);
}
