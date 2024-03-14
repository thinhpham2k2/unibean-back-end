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

    void AddList(IEnumerable<VoucherItem> creations);

    long CountVoucherItemToday(string brandId, DateOnly date);

    bool CheckVoucherCode(string code);

    void Delete(string id);

    PagedResultModel<VoucherItem> GetAll
        (List<string> campaignIds, List<string> campaignDetailIds, List<string> voucherIds, List<string> brandIds, 
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? isBought, bool? isUsed, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherItem GetById(string id);

    VoucherItem GetByVoucherCode(string code);

    ItemIndex GetIndex
        (string voucherId, int quantity, int fromIndex);

    int GetMaxIndex(string voucherId);

    VoucherItem Update(VoucherItem update);

    void UpdateList
        (string voucherId, string campaignDetailId, 
        int quantity, DateOnly StartOn, DateOnly EndOn, ItemIndex index);
}
