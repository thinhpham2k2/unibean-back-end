using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignCampuses;

[ModelBinder(typeof(MetadataValueModelBinder))]
public class CreateCampaignCampusModel
{
    [ValidCampus]
    [Required(ErrorMessage = "Campus is required")]
    public string CampusId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
