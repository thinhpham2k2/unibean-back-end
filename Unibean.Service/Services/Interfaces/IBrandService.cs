using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.Vouchers;

namespace Unibean.Service.Services.Interfaces;

public interface IBrandService
{
    Task<BrandExtraModel> Add(CreateBrandModel creation);

    BrandModel AddGoogle(CreateBrandGoogleModel creation);

    void Delete(string id);

    PagedResultModel<BrandModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, 
        int page, int limit, JwtRequestModel request);

    BrandExtraModel GetById(string id, JwtRequestModel request);

    PagedResultModel<CampaignModel> GetCampaignListByBrandId
        (string id, List<string> typeIds, List<string> storeIds, List<string> majorIds, List<string> campusIds,
        List<CampaignState> stateIds, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<TransactionModel> GetHistoryTransactionListByBrandId
        (string id, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    PagedResultModel<StoreModel> GetStoreListByBrandId
        (string id, List<string> areaIds, bool? state, string propertySort, bool isAsc, 
        string search, int page, int limit);

    PagedResultModel<VoucherModel> GetVoucherListByBrandId
        (string id, List<string> typeIds, bool? state, string propertySort, bool isAsc, 
        string search, int page, int limit);

    Task<BrandExtraModel> Update(string id, UpdateBrandModel update);
}
