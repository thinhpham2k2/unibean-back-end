using Unibean.Repository.Paging;
using Unibean.Service.Models.Vouchers;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherService
{
    PagedResultModel<VoucherModel> GetAll
        (List<string> brandIds, List<string> typeIds, string propertySort,
        bool isAsc, string search, int page, int limit);

    PagedResultModel<VoucherModel> GetAllByStore
        (List<string> storeIds, List<string> campaignIds, List<string> typeIds, 
        string propertySort, bool isAsc, string search, int page, int limit);
}
