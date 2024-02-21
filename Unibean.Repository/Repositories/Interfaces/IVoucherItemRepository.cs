using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public record ItemIndex
{
    public int? FromIndex { get; set; }
    public int? ToIndex { get; set; }
}

public interface IVoucherItemRepository
{
    VoucherItem Add(VoucherItem creation);

    void Delete(string id);

    PagedResultModel<VoucherItem> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, 
        List<string> typeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    List<VoucherItem> GetAllByCampaign
        (List<string> campaignIds, List<string> voucherIds, int limit);

    VoucherItem GetById(string id);

    VoucherItem GetByVoucherCode(string code);

    ItemIndex GetIndex
        (string voucherId, int quantity);

    VoucherItem Update(VoucherItem update);

    void UpdateList
        (string voucherId, string campaignDetailId, 
        int quantity, DateOnly StartOn, DateOnly EndOn, ItemIndex index);
}
