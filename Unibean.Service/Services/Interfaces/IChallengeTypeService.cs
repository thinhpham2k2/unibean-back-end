using Unibean.Repository.Paging;
using Unibean.Service.Models.ChallengeTypes;

namespace Unibean.Service.Services.Interfaces;

public interface IChallengeTypeService
{
    Task<ChallengeTypeModel> Add(CreateChallengeTypeModel creation);

    void Delete(string id);

    PagedResultModel<ChallengeTypeModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    ChallengeTypeModel GetById(string id);

    Task<ChallengeTypeModel> Update(string id, UpdateChallengeTypeModel update);
}
