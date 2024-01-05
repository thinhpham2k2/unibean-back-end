using Unibean.Repository.Paging;
using Unibean.Service.Models.Orders;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class OrderService : IOrderService
{
    public PagedResultModel<OrderModel> GetAll(List<string> stationIds, List<string> studentIds, List<string> stateIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        throw new NotImplementedException();
    }
}
