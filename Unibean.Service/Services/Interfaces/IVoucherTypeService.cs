using Unibean.Repository.Paging;
using Unibean.Service.Models.VoucherTypes;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherTypeService
{
    Task<VoucherTypeModel> Add(CreateVoucherTypeModel creation);

    void Delete(string id);

    PagedResultModel<VoucherTypeModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    VoucherTypeModel GetById(string id);

    Task<VoucherTypeModel> Update(string id, UpdateVoucherTypeModel update);
}
