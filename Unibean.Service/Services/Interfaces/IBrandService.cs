using Unibean.Repository.Paging;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Types;

namespace Unibean.Service.Services.Interfaces;

public interface IBrandService
{
    Task<BrandModel> Add(CreateTypeModel creation);

    void Delete(string id);

    PagedResultModel<BrandModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    BrandExtraModel GetById(string id);

    Task<BrandModel> Update(string id, UpdateTypeModel update);

    BrandModel AddGoogle(CreateBrandGoogleModel creation);
}
