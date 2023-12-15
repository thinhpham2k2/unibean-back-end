using Unibean.Repository.Paging;
using Unibean.Service.Models.Types;

namespace Unibean.Service.Services.Interfaces;

public interface ITypeService
{
    Task<TypeModel> Add(CreateTypeModel creation);

    void Delete(string id);

    PagedResultModel<TypeModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    TypeModel GetById(string id);

    Task<TypeModel> Update(string id, UpdateTypeModel update);
}
