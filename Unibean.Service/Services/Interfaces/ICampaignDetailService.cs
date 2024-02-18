using Unibean.Repository.Paging;
using Unibean.Service.Models.CampaignDetails;

namespace Unibean.Service.Services.Interfaces;

public interface ICampaignDetailService
{
    PagedResultModel<CampaignDetailModel> GetAll
        (List<string> campaignIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<CampaignDetailModel> GetAllByStore
        (string storeId, List<string> campaignIds, List<string> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    List<string> GetAllVoucherItemByCampaignDetail(string id);

    CampaignDetailExtraModel GetById(string id);
}
