using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Campuses;

public class UpdateCampusModel
{
    [ValidUniversity]
    [Required(ErrorMessage = "University is required!")]
    public string UniversityId { get; set; }

    [ValidArea]
    [Required(ErrorMessage = "Area is required!")]
    public string AreaId { get; set; }

    [Required(ErrorMessage = "Campus's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of campus's name is from 3 to 255 characters")]
    public string CampusName { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Address { get; set; }

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
