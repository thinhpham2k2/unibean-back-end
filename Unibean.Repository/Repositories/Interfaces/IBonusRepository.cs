using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IBonusRepository
{
    Bonus Add(Bonus creation);

    void Delete(string id);

    PagedResultModel<Bonus> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    List<Bonus> GetList
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, string search);

    Bonus GetById(string id);

    Bonus Update(Bonus update);
}
