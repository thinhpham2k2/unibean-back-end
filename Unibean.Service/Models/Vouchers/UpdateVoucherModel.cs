using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Vouchers;

public class UpdateVoucherModel
{
    [ValidBrand]
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public string BrandId { get; set; }

    [ValidVoucherType]
    [Required(ErrorMessage = "Loại khuyến mãi là bắt buộc")]
    public string TypeId { get; set; }

    [Required(ErrorMessage = "Tên khuyến mãi là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên khuyến mãi từ 3 đến 255 ký tự")]
    public string VoucherName { get; set; }

    [Required(ErrorMessage = "Chi phí là bắt buộc")]
    [Range(minimum: 1, maximum: double.MaxValue, ErrorMessage = "Chi phí phải là số dương")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Tỷ lệ là bắt buộc")]
    [Range(minimum: 1, maximum: double.MaxValue, ErrorMessage = "Tỷ lệ phải là số dương")]
    public decimal? Rate { get; set; }

    [Required(ErrorMessage = "Điều kiện là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài của điều kiện phải từ 3 ký tự trở lên")]
    public string Condition { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài mô tả phải từ 3 ký tự trở lên")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
