using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Requests;

public class CreateRequestModel
{
    [ValidBrand]
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public string BrandId { get; set; }

    [Required(ErrorMessage = "Chi phí là bắt buộc")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Chi phí phải là số dương")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài mô tả phải từ 3 ký tự trở lên")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
