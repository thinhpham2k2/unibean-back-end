using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class TypeIdModel
{
    [ValidCampaignType]
    [Required(ErrorMessage = "Loại chiến dịch là bắt buộc")]
    public string TypeId { get; set; }
}
