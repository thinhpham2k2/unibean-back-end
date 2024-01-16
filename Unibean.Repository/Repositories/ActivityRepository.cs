using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ActivityRepository : IActivityRepository
{
    public Activity Add(Activity creation)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public PagedResultModel<Activity> GetAll(List<string> storeIds, List<string> studentIds, List<string> voucherIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        throw new NotImplementedException();
    }

    public Activity GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Activity Update(Activity update)
    {
        throw new NotImplementedException();
    }
}
