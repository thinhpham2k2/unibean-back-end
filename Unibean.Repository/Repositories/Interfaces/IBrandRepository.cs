using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IBrandRepository
{
    Brand Add(Brand creation);

    bool CheckBrandId(string id);

    long CountBrand();

    void Delete(string id);

    PagedResultModel<Brand> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    Brand GetById(string id);

    List<Brand> GetRanking(int limit);

    Brand Update(Brand update);
}
