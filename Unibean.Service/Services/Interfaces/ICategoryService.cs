using Unibean.Repository.Paging;
using Unibean.Service.Models.Categories;

namespace Unibean.Service.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryModel> Add(CreateCategoryModel creation);

    void Delete(string id);

    PagedResultModel<CategoryModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    CategoryModel GetById(string id);

    Task<CategoryModel> Update(string id, UpdateCategoryModel update);
}
