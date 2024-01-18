using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IAdminRepository
{
    Admin Add(Admin creation);

    void Delete(string id);

    PagedResultModel<Admin> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Admin GetById(string id);

    Admin Update(Admin update);
}
