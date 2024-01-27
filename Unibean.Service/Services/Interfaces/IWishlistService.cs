using Unibean.Repository.Paging;
using Unibean.Service.Models.Wishlists;

namespace Unibean.Service.Services.Interfaces;

public interface IWishlistService
{
    PagedResultModel<WishlistModel> GetAll
        (List<string> studentIds, List<string> brandIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    WishlistModel UpdateWishlist(UpdateWishlistModel update);
}
