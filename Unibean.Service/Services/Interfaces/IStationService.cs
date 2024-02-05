using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.Stations;

namespace Unibean.Service.Services.Interfaces;

public interface IStationService
{
    Task<StationExtraModel> Add(CreateStationModel creation);

    void Delete(string id);

    PagedResultModel<StationModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    StationExtraModel GetById(string id);

    PagedResultModel<OrderModel> GetOrderListByStudentId
        (string id, List<string> studentIds, List<State> stateIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    Task<StationExtraModel> Update(string id, UpdateStationModel update);
}
