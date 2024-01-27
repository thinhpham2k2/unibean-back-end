using Unibean.Repository.Paging;
using Unibean.Service.Models.Stations;

namespace Unibean.Service.Services.Interfaces;

public interface IStationService
{
    Task<StationModel> Add(CreateStationModel creation);

    void Delete(string id);

    PagedResultModel<StationModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    StationModel GetById(string id);

    Task<StationModel> Update(string id, UpdateStationModel update);
}
