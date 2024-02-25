using Unibean.Repository.Paging;
using Unibean.Service.Models.Staffs;

namespace Unibean.Service.Services.Interfaces;

public interface IStaffService
{
    Task<StaffExtraModel> Add(CreateStaffModel creation);

    void Delete(string id);

    PagedResultModel<StaffModel> GetAll
        (List<string> stationIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    StaffExtraModel GetById(string id);

    Task<StaffExtraModel> Update(string id, UpdateStaffModel update);
}
