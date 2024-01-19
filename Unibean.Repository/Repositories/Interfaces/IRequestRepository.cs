using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IRequestRepository
{
    Request Add(Request creation);

    void Delete(string id);

    PagedResultModel<Request> GetAll
        (List<string> brandIds, List<string> adminIds, string propertySort, bool isAsc, string search, int page, int limit);

    Request GetById(string id);

    Request Update(Request update);
}
