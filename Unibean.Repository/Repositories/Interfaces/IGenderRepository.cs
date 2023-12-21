using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IGenderRepository
{
    Gender Add(Gender creation);

    void Delete(string id);

    PagedResultModel<Gender> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Gender GetById(string id);

    Gender Update(Gender update);
}
