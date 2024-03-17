using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IStoreService
{
    Task<StoreExtraModel> Add(CreateStoreModel creation);

    bool AddActivity
        (string id, string code, CreateUseActivityModel creation);

    void Delete(string id);

    PagedResultModel<StoreModel> GetAll
        (List<string> brandIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<StoreModel> GetAllByCampaign
        (List<string> campaignIds, List<string> brandIds, List<string> areaIds,
         bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    StoreExtraModel GetById(string id);

    PagedResultModel<StoreTransactionModel> GetHistoryTransactionListByStoreId
        (string id, List<StoreTransactionType> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    PagedResultModel<CampaignDetailModel> GetCampaignDetailByStoreId
        (string id, List<string> campaignIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    Task<StoreExtraModel> Update(string id, UpdateStoreModel update);

    CampaignDetailExtraModel GetCampaignDetailById(string id, string detailId);

    VoucherItemExtraModel GetVoucherItemByCode(string id, string code);
}
