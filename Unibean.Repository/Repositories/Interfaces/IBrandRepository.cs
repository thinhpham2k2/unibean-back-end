using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IBrandRepository
{
    Brand Add(Brand creation);

    void Delete(string id);

    PagedResultModel<Brand> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Brand GetById(string id);

    Brand Update(Brand update);
}
