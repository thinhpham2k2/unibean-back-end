using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IProductRepository
{
    Product Add(Product creation);

    long CountProduct();

    void Delete(string id);

    PagedResultModel<Product> GetAll
        (List<string> categoryIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    Product GetById(string id);

    Product Update(Product update);
}
