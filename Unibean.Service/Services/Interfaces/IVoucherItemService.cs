using Microsoft.AspNetCore.Http;
using Unibean.Repository.Paging;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IVoucherItemService
{
    MemoryStream Add(CreateVoucherItemModel creation);

    MemoryStream AddTemplate(InsertVoucherItemModel insert);

    void Delete(string id);

    PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, 
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    MemoryStream GetTemplateVoucherItem();

    VoucherItemExtraModel GetById(string id);
}
