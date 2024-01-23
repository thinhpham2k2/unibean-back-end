using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignMajors;

[ModelBinder(typeof(MetadataValueModelBinder))]
public class CreateCampaignMajorModel
{
    [ValidMajor]
    [Required(ErrorMessage = "Major is required")]
    public string MajorId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
