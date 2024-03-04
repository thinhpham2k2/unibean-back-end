using Unibean.Repository.Entities;
using Unibean.Service.Models.Charts;

namespace Unibean.Service.Services.Interfaces;

public interface IChartService
{
    List<LineChartModel> GetLineChart(string id, Role role);

    TitleAdminModel GetTitleAdmin(string adminId);

    TitleBrandModel GetTitleBrand(string brandId);

    TitleStaffModel GetTitleStaff(string staffId);

    TitleStoreModel GetTitleStore(string storeId);
}
