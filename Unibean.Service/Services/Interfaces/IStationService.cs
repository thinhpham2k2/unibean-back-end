using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Stations;

namespace Unibean.Service.Services.Interfaces;

public interface IStationService
{
    Task<StationExtraModel> Add(CreateStationModel creation);

    void Delete(string id);

    PagedResultModel<StationModel> GetAll
        (List<StationState> stateIds, string propertySort, bool isAsc, string search, int page, int limit);

    StationExtraModel GetById(string id);

    Task<StationExtraModel> Update(string id, UpdateStationModel update);

    bool UpdateState(string id, StationState stateId);
}
