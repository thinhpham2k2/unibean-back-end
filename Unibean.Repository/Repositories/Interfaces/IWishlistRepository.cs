using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IWishlistRepository
{
    Wishlist Add(Wishlist creation);

    void Delete(string id);

    PagedResultModel<Wishlist> GetAll
        (List<string> studentIds, List<string> brandIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    Wishlist GetById(string id);

    Wishlist GetByStudentAndBrand(string studentId, string brandId);

    Wishlist Update(Wishlist update);
}
