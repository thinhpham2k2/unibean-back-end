using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Brands;

namespace Unibean.Service.Services.Interfaces;

public interface IBrandService
{
    BrandExtraModel GetById(string id);

    BrandModel AddGoogle(CreateBrandGoogleModel creation);
}
