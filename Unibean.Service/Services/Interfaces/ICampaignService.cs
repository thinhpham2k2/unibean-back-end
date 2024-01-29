using Unibean.Repository.Paging;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Vouchers;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignService
{
    Task<CampaignExtraModel> Add(CreateCampaignModel creation);

    void Delete(string id);

    PagedResultModel<CampaignModel> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds, List<string> majorIds,
        List<string> campusIds, bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    CampaignExtraModel GetById(string id);

    PagedResultModel<CampusModel> GetCampusListByCampaignId
        (string id, List<string> universityIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<MajorModel> GetMajorListByCampaignId
        (string id, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    PagedResultModel<StoreModel> GetStoreListByCampaignId
        (string id, List<string> brandIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherModel GetVoucherById(string id, string voucherId);

    PagedResultModel<VoucherModel> GetVoucherListByCampaignId
        (string id, List<string> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    Task<CampaignExtraModel> Update(string id, UpdateCampaignModel update);

    CampaignExtraModel UpdateState(string id);
}
