using Unibean.Repository.Paging;
using Unibean.Service.Models.States;

namespace Unibean.Service.Services.Interfaces;

public interface IStateService
{
    Task<StateModel> Add(CreateStateModel creation);

    void Delete(string id);

    PagedResultModel<StateModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    StateModel GetById(string id);

    Task<StateModel> Update(string id, UpdateStateModel update);
}
