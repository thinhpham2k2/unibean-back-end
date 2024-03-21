using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public record ProductRanking
{
    public string Name { get; set; }
    public string Image { get; set; }
    public decimal? Total { get; set; }
}

public interface IProductRepository
{
    Product Add(Product creation);

    long CountProduct();

    void Delete(string id);

    PagedResultModel<Product> GetAll
        (List<string> categoryIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    Product GetById(string id);

    List<ProductRanking> GetRankingByStation(string stationId, int limit);

    Product Update(Product update);
}
