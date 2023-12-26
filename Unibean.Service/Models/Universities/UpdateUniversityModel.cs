using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Universities;

public class UpdateUniversityModel
{
    [Required(ErrorMessage = "University's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of university's name is from 3 to 255 characters")]
    public string UniversityName { get; set; }

    [Phone]
    public string Phone { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Link { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
