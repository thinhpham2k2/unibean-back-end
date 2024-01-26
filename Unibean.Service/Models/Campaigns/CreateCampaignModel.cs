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
    [Required(ErrorMessage = "Brand is required")]
    public string BrandId { get; set; }

    [ValidCampaignType]
    [Required(ErrorMessage = "Campaign type is required")]
    public string TypeId { get; set; }

    [Required(ErrorMessage = "Campaign's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of campaign's name is from 3 to 255 characters")]
    public string CampaignName { get; set; }

    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Condition is required")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "The length of condition must be 3 characters or more")]
    public string Condition { get; set; }

    public string Link { get; set; }

    [ValidStartOn]
    [Required(ErrorMessage = "Start on is required")]
    public DateOnly? StartOn { get; set; }

    [ValidEndOn]
    [Required(ErrorMessage = "End on is required")]
    public DateOnly? EndOn { get; set; }

    [ValidTotalIncome]
    [FromForm(Name = "Cost")]
    [Required(ErrorMessage = "Cost is required")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Cost must be a positive number")]
    public decimal? TotalIncome { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "The length of description must be 3 characters or more")]
    public string Description { get; set; }

    [ValidCampaignMajor]
    [Required(ErrorMessage = "Campaign majors is required")]
    public ICollection<CreateCampaignMajorModel> CampaignMajors { get; set; }

    [ValidCampaignStore]
    [Required(ErrorMessage = "Campaign stores is required")]
    public ICollection<CreateCampaignStoreModel> CampaignStores { get; set; }

    [ValidConstraint]
    [ValidCampaignCampus]
    [Required(ErrorMessage = "Campaign campuses is required")]
    public ICollection<CreateCampaignCampusModel> CampaignCampuses { get; set; }

    [ValidVoucherItem]
    [Required(ErrorMessage = "Vouchers is required")]
    public ICollection<CreateVoucherItemModel> Vouchers { get; set; }
}
