using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Campaigns;

public class UpdateCampaignModel
{
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

    [Required(ErrorMessage = "Description is required")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "The length of description must be 3 characters or more")]
    public string Description { get; set; }
}
