using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.CampaignCampuses;
using Unibean.Service.Models.CampaignMajors;
using Unibean.Service.Models.CampaignStores;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Campaigns;

public class CreateCampaignModel
{
    [ValidBrand]
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public string BrandId { get; set; }

    [ValidCampaignType]
    [Required(ErrorMessage = "Loại chiến dịch là bắt buộc")]
    public string TypeId { get; set; }

    [Required(ErrorMessage = "Tên chiến dịch là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên chiến dịch từ 3 đến 255 ký tự")]
    public string CampaignName { get; set; }

    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Điều kiện là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài của điều kiện phải từ 3 ký tự trở lên")]
    public string Condition { get; set; }

    public string Link { get; set; }

    [ValidStartOn]
    [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
    public DateOnly? StartOn { get; set; }

    [ValidEndOn]
    [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
    public DateOnly? EndOn { get; set; }

    [ValidTotalIncome]
    [FromForm(Name = "Cost")]
    [Required(ErrorMessage = "Chi phí là bắt buộc")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Chi phí phải là số dương")]
    public decimal? TotalIncome { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài mô tả phải từ 3 ký tự trở lên")]
    public string Description { get; set; }

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

    [ValidVoucherItem]
    [Required(ErrorMessage = "Danh sách khuyến mãi là bắt buộc")]
    public ICollection<CreateVoucherItemModel> Vouchers { get; set; }
}
