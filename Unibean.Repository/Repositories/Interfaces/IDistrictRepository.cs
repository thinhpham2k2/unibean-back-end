using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IDistrictRepository
{
    District Add(District creation);

    void Delete(string id);

    PagedResultModel<District> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    District GetById(string id);

    District Update(District update);
}
