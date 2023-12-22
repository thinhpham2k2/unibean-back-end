using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Brands;

public class CreateBrandGoogleModel
{
    [ValidAccount]
    [Required(ErrorMessage = "Account is required!")]
    public string AccountId { get; set; }

    [Required(ErrorMessage = "Brand's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of brand's name is from 3 to 255 characters")]
    public string BrandName { get; set; }

    [ValidEmail]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required!")]
    [StringLength(320, MinimumLength = 3,
            ErrorMessage = "Email is invalid")]
    public string Email { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
