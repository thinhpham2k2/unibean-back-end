using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Vouchers;

public class CreateVoucherModel
{
    [ValidBrand]
    [Required(ErrorMessage = "Brand is required")]
    public string BrandId { get; set; }

    [ValidVoucherType]
    [Required(ErrorMessage = "Voucher type is required")]
    public string TypeId { get; set; }

    [Required(ErrorMessage = "Voucher name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of voucher name is from 3 to 255 characters")]
    public string VoucherName { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(minimum: 1, maximum: double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Rate is required")]
    [Range(minimum: 1, maximum: double.MaxValue, ErrorMessage = "Rate must be a positive number")]
    public decimal? Rate { get; set; }

    [Required(ErrorMessage = "Condition is required")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "The length of condition must be 3 characters or more")]
    public string Condition { get; set; }

    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "The length of description must be 3 characters or more")]
    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
