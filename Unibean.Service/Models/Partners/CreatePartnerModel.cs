using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Partners;

public class CreatePartnerModel
{
    [Required(ErrorMessage = "Brand's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of brand's name is from 3 to 255 characters")]
    public string BrandName { get; set; }

    public string Acronym { get; set; }

    [ValidPartnerUserName]
    [Required(ErrorMessage = "User name is required!")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [StringLength(255, MinimumLength = 8,
        ErrorMessage = "The length of password is from 8 to 255 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirm is required!")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string PasswordConfirmed { get; set; }

    public string Address { get; set; }

    public IFormFile Logo { get; set; }

    public IFormFile CoverPhoto { get; set; }

    [EmailAddress]
    [ValidPartnerEmail]
    public string Email { get; set; }

    [StringLength(20, MinimumLength = 3,
        ErrorMessage = "The length of phone is from 3 to 20 characters")]
    public string Phone { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Link { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
