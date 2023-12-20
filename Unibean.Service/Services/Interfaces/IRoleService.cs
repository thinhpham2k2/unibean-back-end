using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Roles;

namespace Unibean.Service.Services.Interfaces;

public interface IRoleService
{
    RoleModel GetRoleByName(string roleName);

    Task<RoleModel> Add(CreateRoleModel creation);

    void Delete(string id);

    PagedResultModel<RoleModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    RoleModel GetById(string id);

    Task<RoleModel> Update(string id, UpdateRoleModel update);
}
