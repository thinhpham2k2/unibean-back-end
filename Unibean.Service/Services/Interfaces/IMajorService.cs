using Unibean.Repository.Paging;
using Unibean.Service.Models.Majors;

namespace Unibean.Service.Services.Interfaces;

public interface IMajorService
{
    Task<MajorModel> Add(CreateMajorModel creation);

    void Delete(string id);

    PagedResultModel<MajorModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    MajorModel GetById(string id);

    Task<MajorModel> Update(string id, UpdateMajorModel update);
}
