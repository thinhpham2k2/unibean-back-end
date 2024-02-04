using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IOrderRepository
{
    Order Add(Order creation);

    void Delete(string id);

    PagedResultModel<Order> GetAll
        (List<string> stationIds, List<string> studentIds, List<State> stateIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    Order GetById(string id);

    Order Update(Order update);
}
