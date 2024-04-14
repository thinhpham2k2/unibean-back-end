using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Stores;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignService
{
    Task<CampaignExtraModel> Add(CreateCampaignModel creation);

    bool AddActivity(string id, string detailId, CreateBuyActivityModel creation);

    void Delete(string id);

    PagedResultModel<CampaignModel> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds,
        List<string> majorIds, List<string> campusIds, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit);

    CampaignExtraModel GetById(string id);

    PagedResultModel<CampaignActivityModel> GetCampaignActivityListByCampaignId
        (string id, List<CampaignState> stateIds, string propertySort,
        bool isAsc, string search, int page, int limit);

    CampaignDetailExtraModel GetCampaignDetailById(string id, string detailId);

    PagedResultModel<CampaignDetailModel> GetCampaignDetailListByCampaignId
        (string id, List<string> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    PagedResultModel<CampusModel> GetCampusListByCampaignId
        (string id, List<string> universityIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<MajorModel> GetMajorListByCampaignId
        (string id, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit);

    PagedResultModel<StoreModel> GetStoreListByCampaignId
        (string id, List<string> brandIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    Task<CampaignExtraModel> Update(string id, UpdateCampaignModel update);

    bool UpdateState(string id, CampaignState stateId, string note, JwtRequestModel request);
}
