using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IVoucherRepository
{
    Voucher Add(Voucher creation);

    void Delete(string id);

    PagedResultModel<Voucher> GetAll
        (List<string> brandIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<Voucher> GetAllByCampaign
        (List<string> campaignIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<Voucher> GetAllByStore
        (List<string> storeIds, List<string> campaignIds, List<string> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    Voucher GetById(string id);

    Voucher Update(Voucher update);
}
