using Unibean.Repository.Paging;
using Unibean.Service.Models.Stores;

namespace Unibean.Service.Services.Interfaces;

public interface IStoreService
{
    Task<StoreModel> Add(CreateStoreModel creation);

    void Delete(string id);

    PagedResultModel<StoreModel> GetAll
        (List<string> brandIds, List<string> areaIds, string propertySort,
        bool isAsc, string search, int page, int limit);

    StoreExtraModel GetById(string id);

    Task<StoreExtraModel> Update(string id, UpdateStoreModel update);
}
