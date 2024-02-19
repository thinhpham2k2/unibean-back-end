using Unibean.Repository.Paging;
using Unibean.Service.Models.VoucherTypes;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherTypeService
{
    Task<VoucherTypeExtraModel> Add(CreateVoucherTypeModel creation);

    void Delete(string id);

    PagedResultModel<VoucherTypeModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    VoucherTypeExtraModel GetById(string id);

    Task<VoucherTypeExtraModel> Update(string id, UpdateVoucherTypeModel update);
}
