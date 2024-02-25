using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;

namespace Unibean.Service.Services.Interfaces;

public interface IAdminService
{
    Task<AdminExtraModel> Add(CreateAdminModel creation);

    void Delete(string id);

    PagedResultModel<AdminModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    AdminExtraModel GetById(string id);

    Task<AdminExtraModel> Update(string id, UpdateAdminModel update);
}
