using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ProductRepository : IProductRepository
{
    public Product Add(Product creation)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public PagedResultModel<Product> GetAll
        (List<string> categoryIds, List<string> levelIds, string propertySort, 
        bool isAsc, string search, int page, int limit)
    {
        throw new NotImplementedException();
    }

    public Product GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Product Update(Product update)
    {
        throw new NotImplementedException();
    }
}
