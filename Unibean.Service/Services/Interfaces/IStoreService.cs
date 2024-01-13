using Unibean.Repository.Paging;
using Unibean.Service.Models.Stores;

namespace Unibean.Service.Services.Interfaces;

public interface IStoreService
{
    PagedResultModel<StoreModel> GetAll
        (List<string> brandIds, List<string> areaIds, string propertySort,
        bool isAsc, string search, int page, int limit);
}
