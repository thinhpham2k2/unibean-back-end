using Unibean.Repository.Paging;
using Unibean.Service.Models.Orders;

namespace Unibean.Service.Services.Interfaces;

public interface IOrderService
{
    PagedResultModel<OrderModel> GetAll
        (List<string> stationIds, List<string> studentIds, List<string> stateIds, string propertySort, bool isAsc, string search, int page, int limit);
}
