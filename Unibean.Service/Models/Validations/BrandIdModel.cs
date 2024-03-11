using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class BrandIdModel
{
    [ValidBrand]
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public string BrandId { get; set; }
}
