using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStateRepository
{
    State Add(State creation);

    void Delete(string id);

    PagedResultModel<State> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    State GetById(string id);

    State Update(State update);
}
