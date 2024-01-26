using Unibean.Repository.Paging;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherItemService
{
    PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, List<string> typeIds, List<string> studentIds, 
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherItemExtraModel GetById(string id);
}
