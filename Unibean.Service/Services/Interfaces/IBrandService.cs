using Unibean.Repository.Paging;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.Vouchers;

namespace Unibean.Service.Services.Interfaces;

public interface IBrandService
{
    Task<BrandModel> Add(CreateBrandModel creation);

    BrandModel AddGoogle(CreateBrandGoogleModel creation);

    void Delete(string id);

    PagedResultModel<BrandModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit, JwtRequestModel request);

    BrandExtraModel GetById(string id, JwtRequestModel request);

    PagedResultModel<CampaignModel> GetCampaignListByBrandId
        (string id, List<string> typeIds, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<TransactionModel> GetHistoryTransactionListByStudentId
        (string id, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<StoreModel> GetStoreListByBrandId
        (string id, List<string> areaIds, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<VoucherModel> GetVoucherListByBrandId
        (string id, List<string> typeIds, string propertySort, bool isAsc, string search, int page, int limit);

    Task<BrandExtraModel> Update(string id, UpdateBrandModel update);
}
