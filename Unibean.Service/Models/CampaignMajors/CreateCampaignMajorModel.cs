using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignMajors;

[ModelBinder(typeof(MetadataValueModelBinder))]
public class CreateCampaignMajorModel
{
    [ValidMajor]
    [Required(ErrorMessage = "Chuyên ngành là bắt buộc")]
    public string MajorId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
