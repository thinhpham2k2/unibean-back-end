using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IImageRepository
{
    Image Add(Image creation);

    void Delete(string id);

    PagedResultModel<Image> GetAll
        (List<string> productIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    Image GetById(string id);

    Image Update(Image update);
}
