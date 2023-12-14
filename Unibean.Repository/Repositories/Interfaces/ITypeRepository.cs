using Unibean.Repository.Paging;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ITypeRepository
{
    Type Add(Type creation);

    void Delete(string id);

    PagedResultModel<Type> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Type GetById(string id);

    Type Update(Type update);
}
