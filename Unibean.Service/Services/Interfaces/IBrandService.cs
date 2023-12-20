using Unibean.Service.Models.Brands;

namespace Unibean.Service.Services.Interfaces;

public interface IBrandService
{
    BrandExtraModel GetById(string id);

    Task<BrandExtraModel> Add(CreateBrandModel creation);

    BrandModel AddGoogle(CreateBrandGoogleModel creation);
}
