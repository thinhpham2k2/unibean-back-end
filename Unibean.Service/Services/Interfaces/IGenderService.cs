using Unibean.Repository.Paging;
using Unibean.Service.Models.Genders;

namespace Unibean.Service.Services.Interfaces;

public interface IGenderService
{
    Task<GenderModel> Add(CreateGenderModel creation);

    void Delete(string id);

    PagedResultModel<GenderModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    GenderModel GetById(string id);

    Task<GenderModel> Update(string id, UpdateGenderModel update);
}
