using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IChallengeTypeRepository
{
    ChallengeType Add(ChallengeType creation);

    void Delete(string id);

    PagedResultModel<ChallengeType> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    ChallengeType GetById(string id);

    ChallengeType Update(ChallengeType update);
}
