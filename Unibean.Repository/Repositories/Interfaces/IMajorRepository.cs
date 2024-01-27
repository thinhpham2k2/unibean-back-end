using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IMajorRepository
{
    Major Add(Major creation);

    void Delete(string id);

    PagedResultModel<Major> GetAll
        (bool? state, string propertySort, bool isAsc, 
        string search, int page, int limit);

    PagedResultModel<Major> GetAllByCampaign
        (List<string> campaignIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    Major GetById(string id);

    Major Update(Major update);
}
