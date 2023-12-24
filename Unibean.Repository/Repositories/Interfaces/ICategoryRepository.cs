using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICategoryRepository
{
    Category Add(Category creation);

    void Delete(string id);

    PagedResultModel<Category> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Category GetById(string id);

    Category Update(Category update);
}
