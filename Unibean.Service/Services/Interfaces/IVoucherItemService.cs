using Unibean.Repository.Paging;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherItemService
{
    MemoryStream Add(CreateVoucherItemModel creation);

    void Delete(string id);

    PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, 
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherItemExtraModel GetById(string id);
}
