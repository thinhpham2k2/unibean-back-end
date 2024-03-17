using Unibean.Repository.Paging;
using Unibean.Service.Models.Requests;

namespace Unibean.Service.Services.Interfaces;

public interface IRequestService
{
    RequestExtraModel Add(string id, CreateRequestModel creation);

    PagedResultModel<RequestModel> GetAll
        (List<string> brandIds, List<string> adminIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    RequestExtraModel GetById(string id);
}
