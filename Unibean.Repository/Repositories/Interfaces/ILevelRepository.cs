using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ILevelRepository
{
    Level Add(Level creation);

    void Delete(string id);

    PagedResultModel<Level> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Level GetById(string id);

    Level Update(Level update);
}
