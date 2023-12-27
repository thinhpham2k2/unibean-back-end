using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Majors;

public class UpdateMajorModel
{
    [Required(ErrorMessage = "Major's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of major's name is from 3 to 255 characters")]
    public string MajorName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
