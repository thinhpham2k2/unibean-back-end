using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IVoucherTypeRepository
{
    VoucherType Add(VoucherType creation);

    void Delete(string id);

    PagedResultModel<VoucherType> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    VoucherType GetById(string id);

    VoucherType Update(VoucherType update);
}
