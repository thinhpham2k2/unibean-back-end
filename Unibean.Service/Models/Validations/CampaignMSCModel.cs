using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.CampaignCampuses;
using Unibean.Service.Models.CampaignMajors;
using Unibean.Service.Models.CampaignStores;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class CampaignMSCModel
{
    [ValidCampaignMajor]
    [Required(ErrorMessage = "Danh sách chuyên ngành là bắt buộc")]
    public ICollection<CreateCampaignMajorModel> CampaignMajors { get; set; }

    [ValidCampaignStore]
    [Required(ErrorMessage = "Danh sách cửa hàng là bắt buộc")]
    public ICollection<CreateCampaignStoreModel> CampaignStores { get; set; }

    [ValidConstraint]
    [ValidCampaignCampus]
    [Required(ErrorMessage = "Danh sách cơ sở là bắt buộc")]
    public ICollection<CreateCampaignCampusModel> CampaignCampuses { get; set; }
}
