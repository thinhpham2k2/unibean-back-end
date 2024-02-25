using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStaffRepository
{
    Staff Add(Staff creation);

    void Delete(string id);

    PagedResultModel<Staff> GetAll
        (List<string> stationIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    Staff GetById(string id);

    Staff Update(Staff update);
}
