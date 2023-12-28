using Unibean.Repository.Paging;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;

namespace Unibean.Service.Services.Interfaces;

public interface IBrandService
{
    Task<BrandModel> Add(CreateBrandModel creation);

    void Delete(string id);

    PagedResultModel<BrandModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit, JwtRequestModel request);

    BrandExtraModel GetById(string id, JwtRequestModel request);

    Task<BrandModel> Update(string id, UpdateBrandModel update);

    BrandModel AddGoogle(CreateBrandGoogleModel creation);
}
