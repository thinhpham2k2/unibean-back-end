using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStoreRepository
{
    Store Add(Store creation);

    void Delete(string id);

    PagedResultModel<Store> GetAll
        (List<string> brandIds, List<string> areaIds, string propertySort, 
        bool isAsc, string search, int page, int limit);

    Store GetById(string id);

    Store Update(Store update);
}
