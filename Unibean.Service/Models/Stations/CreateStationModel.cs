using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Stations;

public class CreateStationModel
{
    [Required(ErrorMessage = "Station's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of station's name is from 3 to 255 characters")]
    public string StationName { get; set; } 

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    public IFormFile Image { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    [Phone]
    public string Phone { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
