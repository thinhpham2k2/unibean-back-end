using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IUniversityRepository
{
    University Add(University creation);

    void Delete(string id);

    PagedResultModel<University> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    University GetById(string id);

    University Update(University update);
}
