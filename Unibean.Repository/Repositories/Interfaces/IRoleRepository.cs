using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IRoleRepository
{
    Role GetRoleByName(string roleName);

    Role Add(Role creation);

    void Delete(string id);

    PagedResultModel<Role> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    Role GetById(string id);

    Role Update(Role update);
}
