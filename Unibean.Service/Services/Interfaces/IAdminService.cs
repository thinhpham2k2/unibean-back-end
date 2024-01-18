using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;

namespace Unibean.Service.Services.Interfaces;

public interface IAdminService
{
    Task<AdminModel> Add(CreateAdminModel creation);

    void Delete(string id);

    PagedResultModel<AdminModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    AdminModel GetById(string id);

    Task<AdminModel> Update(string id, UpdateAdminModel update);
}
