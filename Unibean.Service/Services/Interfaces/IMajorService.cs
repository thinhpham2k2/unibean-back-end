using Unibean.Repository.Paging;
using Unibean.Service.Models.Majors;

namespace Unibean.Service.Services.Interfaces;

public interface IMajorService
{
    Task<MajorExtraModel> Add(CreateMajorModel creation);

    void Delete(string id);

    PagedResultModel<MajorModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<MajorModel> GetAllByCampaign
        (List<string> campaignIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    MajorExtraModel GetById(string id);

    Task<MajorExtraModel> Update(string id, UpdateMajorModel update);
}
