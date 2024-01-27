using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IChallengeRepository
{
    Challenge Add(Challenge creation);

    void Delete(string id);

    PagedResultModel<Challenge> GetAll
        (List<string> typeIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    Challenge GetById(string id);

    Challenge Update(Challenge update);
}
