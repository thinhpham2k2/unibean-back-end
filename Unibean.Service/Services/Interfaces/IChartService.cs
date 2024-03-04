using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Charts;

namespace Unibean.Service.Services.Interfaces;

public interface IChartService
{
    TitleAdminModel GetTitleAdmin(string adminId);

    TitleBrandModel GetTitleBrand(string brandId);

    TitleStaffModel GetTitleStaff(string staffId);

    TitleStoreModel GetTitleStore(string storeId);
}
