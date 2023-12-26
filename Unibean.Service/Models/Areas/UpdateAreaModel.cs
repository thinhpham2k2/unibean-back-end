using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Areas;

public class UpdateAreaModel
{
    [ValidDistrict]
    [Required(ErrorMessage = "District is required!")]
    public string DistrictId { get; set; }

    [Required(ErrorMessage = "Area's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of area's name is from 3 to 255 characters")]
    public string AreaName { get; set; }

    public IFormFile Image { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
