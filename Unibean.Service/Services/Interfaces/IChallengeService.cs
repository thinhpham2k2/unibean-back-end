using Unibean.Repository.Paging;
using Unibean.Service.Models.Challenges;

namespace Unibean.Service.Services.Interfaces;

public interface IChallengeService
{
    Task<ChallengeModel> Add(CreateChallengeModel creation);

    void Delete(string id);

    PagedResultModel<ChallengeModel> GetAll
        (List<string> typeIds, string propertySort, bool isAsc, string search, int page, int limit);

    ChallengeModel GetById(string id);

    Task<ChallengeModel> Update(string id, UpdateChallengeModel update);
}
