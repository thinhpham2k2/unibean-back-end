using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherItemService
{
    MemoryStream Add(CreateVoucherItemModel creation);

    Task<MemoryStream> AddTemplate(InsertVoucherItemModel insert);

    void Delete(string id);

    PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> campaignDetailIds, List<string> voucherIds, List<string> brandIds,
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? isBought, bool? isUsed, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    MemoryStream GetTemplateVoucherItem();

    VoucherItemExtraModel GetById(string id);

    VoucherItemExtraModel GetByCode(string code);

    VoucherItemExtraModel EntityToExtra(VoucherItem item);
}
