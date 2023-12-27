using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Accounts;

public class CreateBrandAccountModel
{

    [Required(ErrorMessage = "Brand's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of brand's name is from 3 to 255 characters")]
    public string BrandName { get; set; }

    public string Acronym { get; set; }

    [ValidUserName]
    [Required(ErrorMessage = "User name is required")]
    [StringLength(50, MinimumLength = 5,
        ErrorMessage = "The length of user name is from 5 to 50 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 8,
        ErrorMessage = "The length of password is from 8 to 255 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirm is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string PasswordConfirmed { get; set; }

    [Phone]
    [ValidPhone]
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Hash code is required")]
    public string HashCode { get; set; }

    [ValidVerificationCode(nameof(HashCode))]
    [Required(ErrorMessage = "Verification code is required")]
    public string VerificationCode { get; set; }

    [ValidEmail]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    public string Address { get; set; }

    public IFormFile CoverPhoto { get; set; }

    public string Link { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
