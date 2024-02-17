using Unibean.Repository.Paging;
using Unibean.Service.Models.Categories;

namespace Unibean.Service.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryExtraModel> Add(CreateCategoryModel creation);

    void Delete(string id);

    PagedResultModel<CategoryModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    CategoryExtraModel GetById(string id);

    Task<CategoryExtraModel> Update(string id, UpdateCategoryModel update);
}
