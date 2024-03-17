using Unibean.Repository.Paging;
using Unibean.Service.Models.Vouchers;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherService
{
    Task<VoucherExtraModel> Add(CreateVoucherModel creation);

    void Delete(string id);

    PagedResultModel<VoucherModel> GetAll
        (List<string> brandIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherExtraModel GetById(string id);

    Task<VoucherExtraModel> Update(string id, UpdateVoucherModel update);
}
