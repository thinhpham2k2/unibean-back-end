using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IVoucherItemRepository
{
    VoucherItem Add(VoucherItem creation);

    void Delete(string id);

    PagedResultModel<VoucherItem> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, 
        List<string> typeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherItem GetById(string id);

    VoucherItem Update(VoucherItem update);
}
