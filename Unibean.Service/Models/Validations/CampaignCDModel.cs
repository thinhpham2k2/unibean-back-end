using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class CampaignCDModel
{
    [ValidBrand]
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public string BrandId { get; set; }

    [ValidTotalIncome]
    [FromQuery(Name = "Cost")]
    [Required(ErrorMessage = "Chi phí là bắt buộc")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Chi phí phải là số dương")]
    public decimal? TotalIncome { get; set; }

    [ValidCampaignDetail]
    [Required(ErrorMessage = "Chi tiết chiến dịch là bắt buộc")]
    public ICollection<CreateCampaignDetailModel> CampaignDetails { get; set; }
}
