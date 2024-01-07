using Unibean.Repository.Paging;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Products;

namespace Unibean.Service.Services.Interfaces;

public interface IProductService
{
    Task<ProductModel> Add(CreateProductModel creation);

    void Delete(string id);

    PagedResultModel<ProductModel> GetAll
        (List<string> categoryIds, List<string> levelIds, string propertySort, 
        bool isAsc, string search, int page, int limit, JwtRequestModel request);

    ProductExtraModel GetById(string id, JwtRequestModel request);

    Task<ProductExtraModel> Update(string id, UpdateProductModel update);
}
