using Unibean.Repository.Paging;
using Unibean.Service.Models.Areas;

namespace Unibean.Service.Services.Interfaces;

public interface IAreaService
{
    Task<AreaExtraModel> Add(CreateAreaModel creation);

    void Delete(string id);

    PagedResultModel<AreaModel> GetAll
        (bool? state, string propertySort, bool isAsc,
        string search, int page, int limit);

    AreaExtraModel GetById(string id);

    Task<AreaExtraModel> Update(string id, UpdateAreaModel update);
}
