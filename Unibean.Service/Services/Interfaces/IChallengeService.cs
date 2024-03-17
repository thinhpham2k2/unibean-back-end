using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Challenges;

namespace Unibean.Service.Services.Interfaces;

public interface IChallengeService
{
    Task<ChallengeExtraModel> Add(CreateChallengeModel creation);

    void Delete(string id);

    PagedResultModel<ChallengeModel> GetAll
        (List<ChallengeType> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    ChallengeExtraModel GetById(string id);

    Task<ChallengeExtraModel> Update(string id, UpdateChallengeModel update);
}
